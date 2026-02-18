using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class RefTableEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasRefTable
    {
        public byte[] leadingBytes;
        public RefTable Table;

        public RefTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            Table = new RefTable(reader,entryFactory);
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
