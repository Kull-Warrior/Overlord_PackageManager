using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    public record BoneScaleData(Half ScaleX, Half ScaleY, Half ScaleZ);

    class BoneScaleDataArray(uint id, uint relOffset) : ValueEntry<List<BoneScaleData>>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new List<BoneScaleData>((int)(PayloadLength / 6));

            for (int i = 0; i < (PayloadLength / 6); i++)
            {
                Value.Add(new BoneScaleData(reader.ReadHalf(), reader.ReadHalf(), reader.ReadHalf()));
            }
        }
    }
}
