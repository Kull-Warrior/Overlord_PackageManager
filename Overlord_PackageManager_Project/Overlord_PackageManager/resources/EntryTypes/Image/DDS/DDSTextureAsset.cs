using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    class DDSTextureAsset(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, entryFactory);


            foreach (var entry in Table.Entries)
            {
                if(entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is DDSTextureAssetSubTableType1)
                {
                    ((DDSTextureAssetSubTableType1)entry).Read(reader, Table.OffsetOrigin, DDSTextureAssetSubTableType1Dictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public static uint CalculateMipMapCount(uint width, uint height)
        {
            uint maxDim = Math.Max(width, height);
            return (uint)Math.Floor(Math.Log2(maxDim)) + 1;
        }

        public static void WriteDDSHeaderCore(BinaryWriter bw, uint width, uint height, uint mipMapCount, uint pitchOrLinearSize)
        {
            bw.Write(new[] { 'D', 'D', 'S', ' ' });
            bw.Write(0x0000007C);
            bw.Write(0x00021007);
            bw.Write(height);
            bw.Write(width);
            bw.Write(pitchOrLinearSize);
            bw.Write(0x00000000);
            bw.Write(mipMapCount);

            for (int i = 0; i < 11; i++)
                bw.Write(0);
        }

        public static void WriteDXTPixelFormat(BinaryWriter bw, string fourCC)
        {
            bw.Write(0x00000020);
            bw.Write(0x00000004);
            bw.Write(Encoding.ASCII.GetBytes(fourCC));
            bw.Write(0x00000000);
            bw.Write(0x00000000);
            bw.Write(0x00000000);
            bw.Write(0x00000000);
            bw.Write(0x00000000);
        }

        public static void WriteUncompressedRGBPixelFormat(BinaryWriter bw)
        {
            bw.Write(0x00000020);
            bw.Write(0x00000040);
            bw.Write(0x00000000);
            bw.Write(0x00000018);
            bw.Write(0x00FF0000);
            bw.Write(0x0000FF00);
            bw.Write(0x000000FF);
            bw.Write(0x00000000);
        }

        public static void WriteUncompressedRGBAPixelFormat(BinaryWriter bw)
        {
            bw.Write(0x00000020);
            bw.Write(0x00000041);
            bw.Write(0x00000000);
            bw.Write(0x00000020);
            bw.Write(0x00FF0000);
            bw.Write(0x0000FF00);
            bw.Write(0x000000FF);
            bw.Write(0xFF000000);
        }

        public static byte[] CreateDDSHeader(uint width, uint height, uint mipMapCount, DDSFormat format)
        {
            byte[] header = new byte[128];
            using var ms = new MemoryStream(header);
            using var bw = new BinaryWriter(ms);

            uint pitch;
            if (format == DDSFormat.UncompressedRGB)
            {
                // Each pixel takes up 4 Bytes (R, G, B)
                pitch = width * 3;
            }
            else if (format == DDSFormat.UncompressedRGBA)
            {
                // Each pixel takes up 4 Bytes (R, G, B, A)
                pitch = width * 4;
            }
            else
            {
                pitch = 0;
            }

            WriteDDSHeaderCore(bw, width, height, mipMapCount, pitch);

            if (format == DDSFormat.UncompressedRGB)
            {
                WriteUncompressedRGBPixelFormat(bw);
                bw.Write(0x00401000);
            }
            else if (format == DDSFormat.UncompressedRGBA)
            {
                WriteUncompressedRGBAPixelFormat(bw);
                bw.Write(0x00401000);
            }
            else
            {
                WriteDXTPixelFormat(bw, format.ToString());
                bw.Write(0x00401008);
            }

            bw.Write(0x00000000);
            bw.Write(0x00000000);
            bw.Write(0x00000000);
            bw.Write(0x00000000);

            return header;
        }

        public void WriteToFile(string baseDir)
        {
            byte[] fileHeader;
            
            string fileName = ((StringEntry)Table.Entries[1]).varString;
            if (!fileName.ToLower().EndsWith(".dds"))
            {
                fileName += ".dds";
            }

            List<RawDDSTextureData> rawDDSTextures;

            DDSTextureAssetSubTableType1 subTable = (DDSTextureAssetSubTableType1)Table.Entries[3];
            ListOfRawDDSTextureData listOfDDSTextureEntries = (ListOfRawDDSTextureData)subTable.Table.Entries[0];

            rawDDSTextures = listOfDDSTextureEntries.Table.Entries.OfType<RawDDSTextureData>().ToList();

            for (int i = 0; i < rawDDSTextures.Count; i++)
            {
                if (i == 0)
                {
                    uint width = ((Int32Entry)rawDDSTextures[i].Table.Entries[0]).varInt;
                    uint height = ((Int32Entry)rawDDSTextures[i].Table.Entries[1]).varInt;
                    uint rawFormat = ((Int32Entry)rawDDSTextures[i].Table.Entries[2]).varInt;
                    DDSFormat format = (DDSFormat)rawFormat;
                    uint mipMapCount = CalculateMipMapCount(width, height);

                    if (mipMapCount > rawDDSTextures.Count)
                    {
                        mipMapCount = 1;
                    }

                    switch (format)
                    {
                        case DDSFormat.UncompressedRGB:
                        case DDSFormat.UncompressedRGBA:
                        case DDSFormat.DXT1:
                        case DDSFormat.DXT3:
                        case DDSFormat.DXT5:
                            fileHeader = CreateDDSHeader(width, height, mipMapCount, format);
                            break;

                        default:
                            throw new NotSupportedException(
                                $"Unknown DDS format value: {rawFormat}");
                    }
                    using FileStream fileHeaderStream = File.Open(baseDir + fileName, FileMode.Create);
                    using BinaryWriter fileHeaderBinaryWriter = new BinaryWriter(fileHeaderStream);
                    {
                        fileHeaderBinaryWriter.Write(fileHeader);
                    }
                }
                
                byte[] textureData = ((BlobEntry)rawDDSTextures[i].Table.Entries[3]).varBytes;
                
                using FileStream fs = File.Open(baseDir + fileName, FileMode.Append);
                using BinaryWriter br = new BinaryWriter(fs);
                {
                    br.Write(textureData);
                }
            }
        }
    }
}
