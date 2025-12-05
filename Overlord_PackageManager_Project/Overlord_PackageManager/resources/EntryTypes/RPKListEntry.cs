using Overlord_PackageManager.resources.Generic;
using Overlord_PackageManager.resources.Generic.EntryTypes;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class RPKListEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            varRefTable = new RefTable(reader, entryFactory);

            foreach (var entry in varRefTable.Entries)
            {
                ((RPKEntry)entry).Read(reader, varRefTable.origin, 0, RPKDictionary);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
