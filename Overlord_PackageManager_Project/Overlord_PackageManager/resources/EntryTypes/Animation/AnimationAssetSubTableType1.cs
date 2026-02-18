using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class AnimationAssetSubTableType1(uint id, uint relOffset) : Entry(id, relOffset), IHasRefTable
    {
        public RefTable Table;
        public RefTable GetRefTable() => Table;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            Table = new RefTable(reader, entryFactory);


            foreach (var entry in Table.Entries)
            {
                if(entry is ListOfBoneAnimationData)
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
