using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneScaleDataArray(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public BoneScaleData[] boneScales;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            boneScales = new BoneScaleData[PayloadLength / 6];

            for (int i = 0; i < (PayloadLength / 6); i++)
            {
                boneScales[i] = new BoneScaleData();
                boneScales[i].Read(reader);
            }
        }
    }
}
