using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    class BoneScaleListEntry(uint id, uint relOffset) : ValueEntry<List<BoneScale>>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new List<BoneScale>((int)(PayloadLength / 6));

            for (int i = 0; i < (PayloadLength / 6); i++)
            {
                Value.Add(new BoneScale(reader.ReadHalf(), reader.ReadHalf(), reader.ReadHalf()));
            }
        }

        public override long GetPayloadSize()
        {
            return 6;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (BoneScale scaleData in Value)
            {
                writer.Write(scaleData.ScaleX);
                writer.Write(scaleData.ScaleY);
                writer.Write(scaleData.ScaleZ);
            }
        }
    }
}
