using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneRotationDataArray(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public BoneRotationData[] boneRotations;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            boneRotations = new BoneRotationData[PayloadLength / 12];

            for (int i = 0; i < (PayloadLength / 12); i++)
            {
                boneRotations[i] = new BoneRotationData();
                boneRotations[i].Read(reader);
            }
        }
    }
}
