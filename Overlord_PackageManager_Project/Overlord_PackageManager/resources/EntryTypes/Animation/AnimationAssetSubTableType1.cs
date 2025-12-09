using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class AnimationAssetSubTableType1(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public RefTable varRefTable;

        public void Read(BinaryReader reader, long origin, Func<uint, uint, Entry> entryFactory)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varRefTable = new RefTable(reader, entryFactory);


            foreach (var entry in varRefTable.Entries)
            {
                if(entry is ListOfBoneAnimationData)
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
