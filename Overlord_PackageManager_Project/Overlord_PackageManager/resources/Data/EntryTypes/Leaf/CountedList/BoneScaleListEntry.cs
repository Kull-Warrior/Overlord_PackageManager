using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class BoneScaleCountedListEntry(uint id, uint relOffset) : CountedListEntry<BoneScale>(id, relOffset)
    {
        protected override int ElementSize => sizeof(float) * 3; //half ScaleX, half ScaleY, half ScaleZ

        protected override BoneScale ReadValue(BinaryReader reader)
        {
            return new BoneScale(
                reader.ReadHalf(),
                reader.ReadHalf(),
                reader.ReadHalf()
            );
        }

        protected override void WriteValue(BinaryWriter writer, BoneScale value)
        {
            writer.Write(value.ScaleX);
            writer.Write(value.ScaleY);
            writer.Write(value.ScaleZ);
        }
    }
}
