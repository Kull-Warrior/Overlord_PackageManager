using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneAnimationSubTableType25(uint id, uint relOffset) : Entry(id, relOffset), IHasRefTable
    {
        public RefTable Table;
        public RefTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            Table = new RefTable(reader, entryFactory);


            foreach (var entry in Table.Entries)
            {
                if (entry is Int32Entry)
                {
                    entry.Read(reader, Table.origin);
                }
                if (entry is BoneAnimationSubTableType25SubTableType21)
                {
                    ((BoneAnimationSubTableType25SubTableType21)entry).Read(reader, Table.origin, BoneAnimationSubTableType25SubTableType21Dictionary);
                }
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
