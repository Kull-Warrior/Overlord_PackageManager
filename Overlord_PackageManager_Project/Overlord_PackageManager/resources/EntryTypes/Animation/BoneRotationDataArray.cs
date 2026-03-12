using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.Animation
{
    public record BoneRotationData(float Pitch, float Yaw, float Roll);

    class BoneRotationDataArray(uint id, uint relOffset) : ValueEntry<List<BoneRotationData>>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new List<BoneRotationData>((int)(PayloadLength / 12));

            for (int i = 0; i < (PayloadLength / 12); i++)
            {
                Value.Add(new BoneRotationData((float)reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
        }

        public override long GetPayloadSize()
        {
            return 12;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (var scaleData in Value)
            {
                writer.Write(scaleData.Pitch);
                writer.Write(scaleData.Yaw);
                writer.Write(scaleData.Roll);
            }
        }
    }
}
