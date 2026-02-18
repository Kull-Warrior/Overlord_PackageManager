using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class FXEEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasRefTable
    {
        public RefTable Table;
        public RefTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            Table = new RefTable(reader, entryFactory);

            foreach (var entry in Table.Entries)
            {
                if (entry is BinaryEntry)
                {
                    uint length;
                    switch (entry.Id)
                    {
                        case 33:
                            length = 1048576;
                            break;
                        case 37:
                            length = 131072;
                            break;
                        case 39:
                            length = 256;
                            break;
                        default:
                            length = 0;
                            break;
                    }
                    ((BinaryEntry)entry).Read(reader, Table.origin, length);
                }
                else
                {
                    entry.Read(reader, Table.origin);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
