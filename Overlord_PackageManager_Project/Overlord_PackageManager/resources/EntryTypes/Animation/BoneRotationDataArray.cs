using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BonePositionDataArray(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public BonePositionData[] bonePositions;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            bonePositions = new BonePositionData[PayloadLength / 16];

            for (int i = 0; i < (PayloadLength / 16); i++)
            {
                bonePositions[i] = new BonePositionData();
                bonePositions[i].Read(reader);
            }
        }
    }
}
