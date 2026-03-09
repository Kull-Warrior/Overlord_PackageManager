using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Lua
{
    class LuaListEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public ReferenceTable Table;

        public ReferenceTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end, entryFactory);

            foreach (var entry in Table.Entries)
            {
                ((LuaEntry)entry).Read(reader, Table.PayloadStartOffset, LuaDataDictionary);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
