using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class UnknownTableType21Entry(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end, entryFactory);


            foreach (var entry in Table.Entries)
            {
                /*if (entry is DataSubTableType21SubTableType20Entry)
                {
                    ((DataSubTableType21SubTableType20ListEntry)entry).Read(reader, varRefTable.origin, 3, GameObjectDataSubTableType20ListDictionary);
                }*/
                if (entry is Int32Entry || entry is SingleByteEntry)
                {
                    entry.Read(reader, Table.PayloadStartOffset);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
