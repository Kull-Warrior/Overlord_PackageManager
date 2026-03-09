using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class FXEEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.PayloadStartOffset);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
