using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class ListOfBoneAnimationData(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public byte[] leadingBytes;
        public RefTable varRefTable;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            leadingBytes = reader.ReadBytes(3);
            varRefTable = new RefTable(reader);

            foreach (var entry in varRefTable.Entries)
            {
                if (entry is BoneAnimationData)
                {
                    ((BoneAnimationData)entry).Read(reader, varRefTable.origin, 0, BoneAnimationDataDictionary);
                }
            }
        }
    }
}
