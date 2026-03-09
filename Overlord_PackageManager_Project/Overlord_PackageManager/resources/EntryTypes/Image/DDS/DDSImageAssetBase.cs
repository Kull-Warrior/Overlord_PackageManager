using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public abstract class DDSImageAssetBase(uint id, uint relOffset) : AssetEntry(id, relOffset), IFileExportable
    {
        public void Read(BinaryReader reader, long origin, Func<BinaryReader, uint, uint, Entry> factory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            TypeIdentifier = reader.ReadUInt32();
            Table = new ReferenceTable(reader, end, factory);

            foreach (var entry in Table.Entries)
            {
                if (entry is StringEntry or Int32Entry)
                    entry.Read(reader, Table.PayloadStartOffset);

                if (entry is DDSTextureAssetDataContainer container)
                    container.Read(reader, Table.PayloadStartOffset, DDSTextureAssetDataContainerDictionary);
            }
        }

        public abstract void ReplaceFromDDS(byte[] fileBytes);
        public abstract void WriteToDDS(Stream output);

        protected AssetList GetTextureList()
        {
            DDSTextureAssetDataContainer? dataContainer = Table.Entries.OfType<DDSTextureAssetDataContainer>().FirstOrDefault();
            AssetListContainer? mipContainer = dataContainer.Table.Entries.OfType<AssetListContainer>().FirstOrDefault();
            AssetList? list = mipContainer.Table.Entries.OfType<AssetList>().FirstOrDefault();
            return list;
        }

        public void WriteToFile(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            StringEntry fileNameEntry = (StringEntry)Table.Entries[1];
            string fileName = fileNameEntry.Value;

            if (!fileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                fileName += ".dds";

            string fullPath = Path.Combine(directory, fileName);

            using FileStream fs = File.Create(fullPath);
            {
                WriteToDDS(fs);
            }
        }
    }
}