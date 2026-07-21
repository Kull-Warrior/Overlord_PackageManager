using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class DoubleArrayEntry(uint id, uint relOffset) : RawArrayEntry<double>(id, relOffset)
    {
        protected override int ElementSize => sizeof(double);

        protected override double ReadValue(BinaryReader reader) => reader.ReadDouble();

        protected override void WriteValue(BinaryWriter writer, double value) => writer.Write(value);
    }
}