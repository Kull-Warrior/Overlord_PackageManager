using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Lua
{
    class LuaEntry(uint id, uint relOffset) : Entry(id, relOffset), IHasReferenceTable
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
                if(entry is StringEntry || entry is StringArrayEntry || entry is Int32Entry)
                {
                    entry.Read(reader, Table.OffsetOrigin);
                }
                if (entry is BlobEntry)
                {
                    Int32Entry? intEntry = Table.Entries.OfType<Int32Entry>().LastOrDefault();
                    
                    if (intEntry == null)
                        throw new InvalidOperationException("No ByteCode length found");

                    ((BlobEntry)entry).Read(reader, Table.OffsetOrigin, intEntry.varInt);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
