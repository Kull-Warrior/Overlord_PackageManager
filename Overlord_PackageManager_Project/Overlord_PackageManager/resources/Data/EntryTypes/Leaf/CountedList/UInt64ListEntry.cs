using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class UInt64CountedListEntry(uint id, uint relOffset) : CountedListEntry<ulong>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ulong);

        protected override ulong ReadValue(BinaryReader reader) => reader.ReadUInt64();

        protected override void WriteValue(BinaryWriter writer, ulong value) => writer.Write(value);
    }
}