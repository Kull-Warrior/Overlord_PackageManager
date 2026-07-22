using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class FloatCountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<float>(id, relOffset)
    {
        protected override int ElementSize => sizeof(float);

        protected override float ReadValue(BinaryReader reader) => reader.ReadSingle();

        protected override void WriteValue(BinaryWriter writer, float value) => writer.Write(value);
    }
}