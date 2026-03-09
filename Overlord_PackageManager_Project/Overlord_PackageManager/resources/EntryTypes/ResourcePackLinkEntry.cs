using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class ResourcePackLinkEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            leadingBytes = reader.ReadBytes(3);
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                //((ResourcePackRootEntry)entry).Read(reader, Table.origin, 0, RPKDictionary);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
