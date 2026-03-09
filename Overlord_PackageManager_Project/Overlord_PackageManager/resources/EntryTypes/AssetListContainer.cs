using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class AssetListContainer(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        public void Read(BinaryReader reader, long origin, Func<BinaryReader, uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end, entryFactory);


            foreach (var entry in Table.Entries)
            {
                ((AssetList)entry).Read(reader, Table.PayloadStartOffset, Entry.AssetListDictionary);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
