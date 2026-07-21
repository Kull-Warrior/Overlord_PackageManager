using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    class BoneRotationListEntry(uint id, uint relOffset) : ValueEntry<List<BoneRotation>>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new List<BoneRotation>((int)(PayloadLength / 12));

            for (int i = 0; i < (PayloadLength / 12); i++)
            {
                Value.Add(new BoneRotation((float)reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
        }

        public override long GetPayloadSize()
        {
            return 12;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (BoneRotation scaleData in Value)
            {
                writer.Write(scaleData.Pitch);
                writer.Write(scaleData.Yaw);
                writer.Write(scaleData.Roll);
            }
        }
    }
}
