using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class Int64CountedListEntry(uint id, uint relOffset) : CountedListEntry<long>(id, relOffset)
    {
        protected override int ElementSize => sizeof(long);

        protected override long ReadValue(BinaryReader reader) => reader.ReadInt64();

        protected override void WriteValue(BinaryWriter writer, long value) => writer.Write(value);
    }
}