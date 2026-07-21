using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class UInt64ListEntry(uint id, uint relOffset) : RawListEntry<ulong>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ulong);

        protected override ulong ReadValue(BinaryReader reader) => reader.ReadUInt64();

        protected override void WriteValue(BinaryWriter writer, ulong value) => writer.Write(value);
    }
}