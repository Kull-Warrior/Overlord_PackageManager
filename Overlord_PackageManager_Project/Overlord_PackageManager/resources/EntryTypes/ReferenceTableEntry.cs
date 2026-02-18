using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class ReferenceTableEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;

        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            Table = new ReferenceTable(reader,entryFactory);
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
