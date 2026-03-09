using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Image.DDS
{
    class DDSTextureAssetDataContainer(uint id, uint relOffset) : TableEntry(id, relOffset)
    {
        public void Read(BinaryReader reader, long origin, Func<BinaryReader, uint, uint, Entry> entryFactory)
        {
            long start = origin + RelativeOffset;
            long end = start + PayloadLength;

            reader.BaseStream.Position = start;
            Table = new ReferenceTable(reader, end, entryFactory);


            foreach (var entry in Table.Entries)
            {
                if (entry is AssetListContainer)
                {
                    ((AssetListContainer)entry).Read(reader, Table.PayloadStartOffset, Entry.AssetListContainerDictionary);
                }
                else
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
