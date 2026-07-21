using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class Int64ListEntry(uint id, uint relOffset) : RawListEntry<long>(id, relOffset)
    {
        protected override int ElementSize => sizeof(long);

        protected override long ReadValue(BinaryReader reader) => reader.ReadInt64();

        protected override void WriteValue(BinaryWriter writer, long value) => writer.Write(value);
    }
}