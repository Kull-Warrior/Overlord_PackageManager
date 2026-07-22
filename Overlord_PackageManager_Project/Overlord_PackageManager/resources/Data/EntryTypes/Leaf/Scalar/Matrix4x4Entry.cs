using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class Matrix4x4Entry(uint id, uint relOffset) : ScalarEntry<Matrix4x4>(id, relOffset)
    {
        protected override int ElementSize => 16 * sizeof(float);

        protected override Matrix4x4 ReadValue(BinaryReader reader)
        {
            return new Matrix4x4(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, Matrix4x4 value)
        {
            writer.Write(value.M11); writer.Write(value.M12); writer.Write(value.M13); writer.Write(value.M14);
            writer.Write(value.M21); writer.Write(value.M22); writer.Write(value.M23); writer.Write(value.M24);
            writer.Write(value.M31); writer.Write(value.M32); writer.Write(value.M33); writer.Write(value.M34);
            writer.Write(value.M41); writer.Write(value.M42); writer.Write(value.M43); writer.Write(value.M44);
        }
    }
}