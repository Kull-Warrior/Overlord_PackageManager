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

        private string GetAssetName()
        {
            uint swappedTypeIdentifier = BinaryPrimitives.ReverseEndianness(TypeIdentifier);
            string rawAssetName = $"UnknownAssetType_{swappedTypeIdentifier:X8}";
            
            if (Table.Entries.Count > 1 && Table.Entries[0] != null && Table.Entries[1] != null)
            {
                ReadOnlySpan<byte> gameTagSpan = ((RawArrayEntry<byte>)Table.Entries[0]).Value;
                ReadOnlySpan<byte> assetNameSpan = ((RawArrayEntry<byte>)Table.Entries[1]).Value;
                if (gameTagSpan.Length > 4)
                {
                    gameTagSpan = gameTagSpan.Slice(4);
                }
                if (assetNameSpan.Length > 4)
                {
                    assetNameSpan = assetNameSpan.Slice(4);
                }
                string rawString = System.Text.Encoding.ASCII.GetString(gameTagSpan);
                int closingBracket = rawString.IndexOf(']');
                if (closingBracket >= 0)
                {
                    int slash = rawString.IndexOf('\\', closingBracket + 1);
                    if (slash > closingBracket)
                    {
                        string assetType = rawString.Substring(closingBracket + 1, slash - closingBracket - 1);
                        string assetName = System.Text.Encoding.ASCII.GetString(assetNameSpan);
                        rawAssetName = $"{rawAssetName}_{assetName}_{assetType}";
                    }
                }
            }
            return rawAssetName;
        }

        public void WriteToFile(string baseDir)
        {
            string assetDirectory = Path.Combine(baseDir, GetAssetName());
            Directory.CreateDirectory(assetDirectory);

            foreach (var entry in Table.Entries)
            {
                if (entry is ValueEntry<byte[]> valueEntry)
                {
                    string fileName = Path.Combine(assetDirectory, $"{entry.Id:X8}.bin");
                    using FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                    {
                        fs.Write(valueEntry.Value);
                    }
                }
            }
        }
    }
}