using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using System.Buffers.Binary;
using System.IO;

namespace Overlord_PackageManager.resources.Data.Generic
{
    public class AssetEntry : TableEntry
    {
        public uint TypeIdentifier { get; set; }

        protected override int PayloadOffset => 4;

        public AssetEntry(uint id, uint relOffset, uint typeIdentifier) : base(id, relOffset)
        {
            TypeIdentifier = typeIdentifier;
        }

        protected override Func<BinaryReader, uint, uint, Entry> EntryFactory => CreateUnkownAsset;

        public static Entry CreateUnkownAsset(BinaryReader reader, uint id, uint relOffset)
        {
            return id switch
            {
                // Add more IDs here
                _ => new RawArrayEntry<byte>(id, relOffset, BinaryTypes.Byte),   // Unknown entry
            };
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            long start = origin + RelativeOffset;
            writer.BaseStream.Position = start;
            writer.Write(TypeIdentifier);
            long tableStart = start + PayloadOffset;
            writer.BaseStream.Position = tableStart;
            Table.Write(writer, tableStart);
        }

        private string GetName()
        {
            uint swappedTypeIdentifier = BinaryPrimitives.ReverseEndianness(TypeIdentifier);
            return $"UnknownAsset_Type-{swappedTypeIdentifier:X8}_TableID-{Id:X8}";
        }

        public void WriteToFile(string baseDir)
        {
            string assetDirectory = Path.Combine(baseDir, GetName());
            Directory.CreateDirectory(assetDirectory);

            foreach (var entry in Table.Entries)
            {
                if (entry is ValueEntry<byte[]> valueEntry)
                {
                    string fileName = Path.Combine(assetDirectory, $"{entry.Id:X8}.bin");
                    using FileStream fs = new FileStream(fileName, FileMode.Create);
                    {
                        fs.Write(valueEntry.Value);
                    }
                }
            }
        }
    }
}