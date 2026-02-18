using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Lua
{
    class LuaListEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;

        public ReferenceTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            Table = new ReferenceTable(reader, entryFactory);

            foreach (var entry in Table.Entries)
            {
                ((LuaEntry)entry).Read(reader, Table.OffsetOrigin, 19, LuaDataDictionary);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
