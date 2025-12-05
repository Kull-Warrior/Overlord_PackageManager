using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes
{
    class UnknownTableType21Entry : Entry
    {
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public UnknownTableType21Entry(uint id, uint relOffset) : base(id, relOffset)
        {

        }

        public void Read(BinaryReader reader, long origin, uint numberOfLeadingBytes, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes((int)numberOfLeadingBytes);
            varRefTable = new RefTable(reader, entryFactory);


            foreach (var entry in varRefTable.Entries)
            {
                /*if (entry is DataSubTableType21SubTableType20Entry)
                {
                    ((DataSubTableType21SubTableType20ListEntry)entry).Read(reader, varRefTable.origin, 3, GameObjectDataSubTableType20ListDictionary);
                }*/
                if (entry is Int32Entry || entry is ByteEntry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
