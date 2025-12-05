using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Audio
{
    class SFXAsset(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] identifier;
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            identifier = reader.ReadBytes(4);
            varRefTable = new RefTable(reader, entryFactory);


            foreach (var entry in varRefTable.Entries)
            {
                if(entry is StringEntry || entry is Int32Entry)
                {
                    entry.Read(reader, varRefTable.origin);
                }
                if (entry is BinaryEntry)
                {
                    Int32Entry? intEntry = varRefTable.Entries.OfType<Int32Entry>().LastOrDefault();
                    
                    if (intEntry == null)
                        throw new InvalidOperationException("No ByteCode length found");

                    ((BinaryEntry)entry).Read(reader, varRefTable.origin, intEntry.varInt);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
