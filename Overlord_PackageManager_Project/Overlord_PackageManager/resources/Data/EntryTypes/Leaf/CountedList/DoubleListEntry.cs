using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class DoubleCountedListEntry(uint id, uint relOffset) : CountedListEntry<double>(id, relOffset)
    {
        protected override int ElementSize => sizeof(double);

        protected override double ReadValue(BinaryReader reader) => reader.ReadDouble();

        protected override void WriteValue(BinaryWriter writer, double value) => writer.Write(value);
    }
}