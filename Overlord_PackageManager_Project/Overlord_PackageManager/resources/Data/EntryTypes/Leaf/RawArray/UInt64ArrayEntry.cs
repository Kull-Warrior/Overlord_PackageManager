using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class UInt64ArrayEntry(uint id, uint relOffset) : RawArrayEntry<ulong>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ulong);

        protected override ulong ReadValue(BinaryReader reader) => reader.ReadUInt64();

        protected override void WriteValue(BinaryWriter writer, ulong value) => writer.Write(value);
    }
}