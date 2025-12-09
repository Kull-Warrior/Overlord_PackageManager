using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneRotationDataArray(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public BoneRotationData[] boneRotations;

        public void Read(BinaryReader reader, long origin, uint length)
        {
            reader.BaseStream.Position = origin + RelOffset;
            boneRotations = new BoneRotationData[length];

            for (int i = 0; i < length; i++)
            {
                boneRotations[i] = new BoneRotationData();
                boneRotations[i].Read(reader);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
