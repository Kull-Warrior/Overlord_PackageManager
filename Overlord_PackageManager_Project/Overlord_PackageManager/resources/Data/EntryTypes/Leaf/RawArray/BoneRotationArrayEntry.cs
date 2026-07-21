using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class BoneRotationArrayEntry(uint id, uint relOffset) : RawArrayEntry<BoneRotation>(id, relOffset)
    {
        protected override int ElementSize => sizeof(float) * 3; //float Pitch, float Yaw, float Roll

        protected override BoneRotation ReadValue(BinaryReader reader)
        {
            return new BoneRotation(
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, BoneRotation value)
        {
            writer.Write(value.Pitch);
            writer.Write(value.Yaw);
            writer.Write(value.Roll);
        }
    }
}