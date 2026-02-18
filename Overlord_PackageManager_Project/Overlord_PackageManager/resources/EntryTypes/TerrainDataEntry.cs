using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class TerrainDataEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
    {
        public byte[] leadingBytes;
        public ReferenceTable Table;
        public ReferenceTable GetReferenceTable() => Table;

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            Table = new ReferenceTable(reader, entryFactory);

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
                    ((BinaryEntry)entry).Read(reader, Table.OffsetOrigin, length);
                }
                else
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
