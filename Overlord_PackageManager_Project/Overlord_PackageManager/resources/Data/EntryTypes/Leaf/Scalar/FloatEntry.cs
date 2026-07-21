using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class FloatEntry(uint id, uint relOffset) : ScalarEntry<float>(id, relOffset)
    {
        protected override int ElementSize => sizeof(float);

        protected override float ReadValue(BinaryReader reader) => reader.ReadSingle();

        protected override void WriteValue(BinaryWriter writer, float value) => writer.Write(value);
    }
}