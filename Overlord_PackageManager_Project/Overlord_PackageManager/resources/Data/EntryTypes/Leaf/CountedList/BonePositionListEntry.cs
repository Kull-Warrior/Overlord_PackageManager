using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class BonePositionCountedListEntry(uint id, uint relOffset) : CountedListEntry<BonePosition>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint) + sizeof(float) * 3; //uint Timestamp, float X, float Y, float Z

        protected override BonePosition ReadValue(BinaryReader reader)
        {
            return new BonePosition(
                reader.ReadUInt32(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, BonePosition value)
        {
            writer.Write(value.Timestamp);
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
        }
    }
}