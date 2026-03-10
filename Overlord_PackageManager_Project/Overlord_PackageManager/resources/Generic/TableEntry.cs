using System.IO;

namespace Overlord_PackageManager.resources.Generic
{
    public class TableEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table { get; set; }

        public ReferenceTable GetReferenceTable() => Table;

        // Grammar definition for this table type
        protected virtual Func<BinaryReader, uint, uint, Entry> EntryFactory => null;

        protected virtual int PayloadOffset => 0;

        public override void Read(BinaryReader reader, long origin)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start + PayloadOffset;
            Table = new ReferenceTable(reader, end, EntryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
            }
        }
    }
}
