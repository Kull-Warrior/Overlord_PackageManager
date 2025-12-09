using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BonePositionDataArray(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public BonePositionData[] bonePositions;

        public void Read(BinaryReader reader, long origin, uint length)
        {
            reader.BaseStream.Position = origin + RelOffset;
            bonePositions = new BonePositionData[length];

            for (int i = 0; i < length; i++)
            {
                bonePositions[i] = new BonePositionData();
                bonePositions[i].Read(reader);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
