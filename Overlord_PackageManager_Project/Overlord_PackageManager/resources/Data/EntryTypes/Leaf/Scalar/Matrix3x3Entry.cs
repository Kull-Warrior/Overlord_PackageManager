using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class Matrix3x3Entry(uint id, uint relOffset) : ScalarEntry<Matrix3x3>(id, relOffset)
    {
        protected override int ElementSize => 9 * sizeof(float);

        protected override Matrix3x3 ReadValue(BinaryReader reader)
        {
            return new Matrix3x3(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, Matrix3x3 value)
        {
            writer.Write(value.M11); writer.Write(value.M12); writer.Write(value.M13);
            writer.Write(value.M21); writer.Write(value.M22); writer.Write(value.M23);
            writer.Write(value.M31); writer.Write(value.M32); writer.Write(value.M33);
        }
    }
}