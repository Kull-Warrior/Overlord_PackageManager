using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    class BoneScaleDataArray(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public BoneScaleData[] boneScales;

        public void Read(BinaryReader reader, long origin, uint length)
        {
            reader.BaseStream.Position = origin + RelOffset;
            boneScales = new BoneScaleData[length];

            for (int i = 0; i < length; i++)
            {
                boneScales[i] = new BoneScaleData();
                boneScales[i].Read(reader);
            }
        }

        public override void Read(BinaryReader reader, long origin)
        {
            throw new NotImplementedException();
        }
    }
}
