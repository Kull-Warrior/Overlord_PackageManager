using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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
                19 => new Int32Entry(id, relOffset),    // FFFF Block
                20 => new StringEntry(id, relOffset),   // Chunk or In-Game Object Name
                21 => new StringEntry(id, relOffset),   // Asset Name
                // Add more IDs here
                _ => new BlobEntry(id, relOffset),   // Unknown entry
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
    }
}