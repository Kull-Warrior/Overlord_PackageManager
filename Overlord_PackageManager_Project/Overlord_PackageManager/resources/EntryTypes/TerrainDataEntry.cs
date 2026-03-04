using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class TerrainDataEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelOffset;
            long end = start + Length;

            reader.BaseStream.Position = start;
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                entry.Read(reader, Table.OffsetOrigin);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
