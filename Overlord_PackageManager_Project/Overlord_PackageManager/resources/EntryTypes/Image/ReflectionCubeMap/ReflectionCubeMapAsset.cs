using Overlord_PackageManager.resources.EntryEditor;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.ReflectionCubeMap
{
    public class ReflectionCubeMapAsset(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public uint TypeIdentifier;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, entryFactory);


            foreach (Entry entry in Table.Entries)
            {
                if(entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is DDSTextureAssetDataContainer)
                {
                    ((DDSTextureAssetDataContainer)entry).Read(reader, Table.OffsetOrigin, DDSTextureAssetDataContainerDictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }

        public void ParseAndReplaceCubemapDDS(byte[] fileBytes)
        {
            using MemoryStream ms = new(fileBytes);
            using BinaryReader br = new(ms);

            // Check magic
            string magic = new string(br.ReadChars(4));
            if (magic != "DDS ")
                throw new InvalidDataException("Not a valid DDS file");

            br.BaseStream.Position = 8;
            uint flags = br.ReadUInt32();
            uint height = br.ReadUInt32();
            uint width = br.ReadUInt32();
            uint pitchOrLinearSize = br.ReadUInt32();
            uint depth = br.ReadUInt32();
            uint mipMapCount = br.ReadUInt32();

            // Skip reserved
            br.BaseStream.Position = 76;
            uint pfSize = br.ReadUInt32();
            uint pfFlags = br.ReadUInt32();
            string fourCC = new string(br.ReadChars(4));

            DDSFormat format;
            if ((pfFlags & 0x4) != 0) // compressed
            {
                format = fourCC switch
                {
                    "DXT1" => DDSFormat.DXT1,
                    "DXT3" => DDSFormat.DXT3,
                    "DXT5" => DDSFormat.DXT5,
                    _ => throw new NotSupportedException($"Unsupported DDS format {fourCC}")
                };
            }
            else
            {
                br.BaseStream.Position = 88;
                uint bitCount = br.ReadUInt32();
                format = bitCount == 32 ? DDSFormat.UncompressedRGBA : DDSFormat.UncompressedRGB;
            }

            // Skip rest of header
            br.BaseStream.Position = 128;

            // Calculate number of faces (cubemap)
            int faces = 6;
            uint singleMipBytes = DDSTextureAssetEditor.CalculateMipByteSize(width, height, format);
            uint[] mipWidths = new uint[mipMapCount];
            uint[] mipHeights = new uint[mipMapCount];

            uint w = width;
            uint h = height;
            for (int i = 0; i < mipMapCount; i++)
            {
                mipWidths[i] = w;
                mipHeights[i] = h;
                w = Math.Max(1, w / 2);
                h = Math.Max(1, h / 2);
            }

            // Clear current list
            DDSTextureAssetDataContainer container = (DDSTextureAssetDataContainer)Table.Entries[3];
            ListOfDDSTextures list = (ListOfDDSTextures)container.Table.Entries[0];
            list.Table.Entries.Clear();

            // Read all faces and mip levels
            for (int face = 0; face < faces; face++)
            {
                for (int mip = 0; mip < mipMapCount; mip++)
                {
                    uint mipWidth = mipWidths[mip];
                    uint mipHeight = mipHeights[mip];
                    uint byteSize = DDSTextureAssetEditor.CalculateMipByteSize(mipWidth, mipHeight, format);

                    byte[] data = br.ReadBytes((int)byteSize);

                    DDSTextures entry = new DDSTextures(mipWidth, mipHeight, format, data);
                    list.Table.Entries.Add(entry);
                }
            }
        }

        public void WriteToFile(string baseDir)
        {
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            string fileName = ((StringEntry)Table.Entries[1]).varString;
            if (!fileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".dds";
            }

            DDSTextureAssetDataContainer container = (DDSTextureAssetDataContainer)Table.Entries[3];

            ListOfDDSTextures list = (ListOfDDSTextures)container.Table.Entries[0];

            List<DDSTextures> textures = list.Table.Entries.OfType<DDSTextures>().ToList();

            if (textures.Count == 0)
            {
                return;
            }

            uint width = ((Int32Entry)textures[0].Table.Entries[0]).varInt;
            uint height = ((Int32Entry)textures[0].Table.Entries[1]).varInt;
            DDSFormat format = (DDSFormat)((Int32Entry)textures[0].Table.Entries[2]).varInt;

            uint mipCount = DDSWriter.CalculateMipMapCount(width, height);

            byte[] header = DDSWriter.CreateDDSHeader(width, height, mipCount, format, true);

            using FileStream fs = File.Create(Path.Combine(baseDir, fileName));
            using BinaryWriter bw = new BinaryWriter(fs);
            {
                bw.Write(header);

                // Cubemap ordering:
                // Right, Left, Top, Bottom, Front, Back
                // Each face contains all its mip levels sequentially

                int faces = 6;

                if (textures.Count != faces * mipCount)
                {
                    throw new InvalidDataException("Cubemap texture count does not match face*mip layout.");
                }

                for (int face = 0; face < faces; face++)
                {
                    for (int mip = 0; mip < mipCount; mip++)
                    {
                        int index = face * (int)mipCount + mip;

                        BlobEntry blob =
                            textures[index]
                            .Table.Entries
                            .OfType<BlobEntry>()
                            .First();

                        bw.Write(blob.varBytes);
                    }
                }
            }
        }
    }
}
