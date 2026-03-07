using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    public abstract class DDSImageAssetBase : Entry, IHasReferenceTable, IFileExportable
    {
        protected DDSImageAssetBase(uint id, uint relOffset) : base(id, relOffset) { }

        public uint TypeIdentifier;
        public ReferenceTable Table;

        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> factory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            reader.BaseStream.Position = origin + RelativeOffset;
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
            DDSTextureAssetDataContainer container = (DDSTextureAssetDataContainer)Table.Entries[3];
            return (AssetList)container.Table.Entries[0];
        }

        public void WriteToFile(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            StringEntry fileNameEntry = (StringEntry)Table.Entries[1];
            string fileName = fileNameEntry.varString;

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