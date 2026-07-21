using System.IO;
using System.Numerics;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class Vector4ArrayEntry(uint id, uint relOffset) : RawArrayEntry<Vector4>(id, relOffset)
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