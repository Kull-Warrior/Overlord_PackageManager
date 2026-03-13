using System.IO;

namespace Overlord_PackageManager.resources.Data.Generic
{
    public class AssetEntry : TableEntry
    {
        public uint TypeIdentifier { get; set; }

        protected override int PayloadOffset => 4;

        protected AssetEntry(uint id, uint relOffset, uint typeIdentifier) : base(id, relOffset)
        {
            TypeIdentifier = typeIdentifier;
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
