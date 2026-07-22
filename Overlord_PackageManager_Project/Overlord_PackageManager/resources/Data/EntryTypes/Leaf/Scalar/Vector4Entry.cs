using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class Vector4Entry(uint id, uint relOffset) : ScalarEntry<Vector4>(id, relOffset)
    {
        protected override int ElementSize => 4 * sizeof(float);

        protected override Vector4 ReadValue(BinaryReader reader)
        {
            return new Vector4(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
            );
        }

        protected override void WriteValue(BinaryWriter writer, Vector4 value)
        {
            writer.Write(value.X); writer.Write(value.Y); writer.Write(value.Z); writer.Write(value.W);
        }
    }
}