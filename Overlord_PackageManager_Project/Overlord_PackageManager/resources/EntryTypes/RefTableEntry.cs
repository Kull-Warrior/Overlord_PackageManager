using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class RefTableEntry : Entry
    {
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public RefTableEntry(uint id, uint relOffset) : base(id, relOffset)
        {

        }

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            varRefTable = new RefTable(reader,entryFactory);
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
