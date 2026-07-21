using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    class BonePositionListEntry(uint id, uint relOffset) : ValueEntry<List<BonePosition>>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new List<BonePosition>((int)(PayloadLength / 16));

            for (int i = 0; i < (PayloadLength / 16); i++)
            {
                Value.Add(new BonePosition(reader.ReadUInt32(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
        }

        public override long GetPayloadSize()
        {
            return 16;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (BonePosition positionData in Value)
            {
                writer.Write(positionData.Timestamp);
                writer.Write(positionData.X);
                writer.Write(positionData.Y);
                writer.Write(positionData.Z);
            }
        }
    }
}
