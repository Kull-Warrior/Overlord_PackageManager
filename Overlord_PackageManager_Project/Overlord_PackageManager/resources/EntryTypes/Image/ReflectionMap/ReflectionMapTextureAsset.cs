using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.ReflectionMap
{
    class ReflectionMapTextureAsset(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
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

        public void WriteToFile(string baseDir)
        {
            byte[] fileHeader;
            string fileName = "";

            string objectName = ((StringEntry)Table.Entries[1]).varString;
            Directory.CreateDirectory(baseDir + objectName);

            List<RawDDSTextureData> rawDDSTextures;

            DDSTextureAssetSubTableType1 subTable = (DDSTextureAssetSubTableType1)Table.Entries[3];
            ListOfRawDDSTextureData listOfDDSTextureEntries = (ListOfRawDDSTextureData)subTable.Table.Entries[0];

            rawDDSTextures = listOfDDSTextureEntries.Table.Entries.OfType<RawDDSTextureData>().ToList();

            uint baseMipMapCount;
            
            // Determine mip count from first texture
            {
                uint width = ((Int32Entry)rawDDSTextures[0].Table.Entries[0]).varInt;
                uint height = ((Int32Entry)rawDDSTextures[0].Table.Entries[1]).varInt;
                baseMipMapCount = DDSTextureAsset.CalculateMipMapCount(width, height);
            }

            // Sanity check
            if (rawDDSTextures.Count % baseMipMapCount != 0)
            {
                throw new InvalidDataException(
                    "Raw DDS texture count is not a multiple of the mip map count.");
            }

            for (int i = 0; i < rawDDSTextures.Count; i++)
            {
                if (i % baseMipMapCount == 0 && i <= rawDDSTextures.Count - baseMipMapCount)
                {
                    fileName = $"\\ReflectionMap_Section_{i / baseMipMapCount}.dds";
                    uint width = ((Int32Entry)rawDDSTextures[i].Table.Entries[0]).varInt;
                    uint height = ((Int32Entry)rawDDSTextures[i].Table.Entries[1]).varInt;
                    uint rawFormat = ((Int32Entry)rawDDSTextures[i].Table.Entries[2]).varInt;
                    DDSFormat format = (DDSFormat)rawFormat;
                    uint mipMapCount = DDSTextureAsset.CalculateMipMapCount(width, height);

                    switch (format)
                    {
                        case DDSFormat.UncompressedRGBA:
                            fileHeader = DDSTextureAsset.CreateDDSHeader(width, height, mipMapCount, format);
                            break;

                        case DDSFormat.DXT1:
                        case DDSFormat.DXT3:
                        case DDSFormat.DXT5:
                            fileHeader = DDSTextureAsset.CreateDDSHeader(width, height, mipMapCount, format);
                            break;

                        default:
                            throw new NotSupportedException(
                                $"Unknown DDS format value: {rawFormat}");
                    }
                    using FileStream fileHeaderStream = File.Open(baseDir + objectName + fileName, FileMode.Create);
                    using BinaryWriter fileHeaderBinaryWriter = new BinaryWriter(fileHeaderStream);
                    {
                        fileHeaderBinaryWriter.Write(fileHeader);
                    }
                }

                byte[] textureData = ((BinaryEntry)rawDDSTextures[i].Table.Entries[3]).varBytes;

                using FileStream fs = File.Open(baseDir + objectName + fileName, FileMode.Append);
                using BinaryWriter br = new BinaryWriter(fs);
                {
                    br.Write(textureData);
                }
            }
        }
    }
}
