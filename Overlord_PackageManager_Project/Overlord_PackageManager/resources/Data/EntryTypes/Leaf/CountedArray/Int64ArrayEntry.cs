using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class Int64CountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<long>(id, relOffset)
    {
        protected override int ElementSize => sizeof(long);

        protected override long ReadValue(BinaryReader reader) => reader.ReadInt64();

        protected override void WriteValue(BinaryWriter writer, long value) => writer.Write(value);
    }
}
