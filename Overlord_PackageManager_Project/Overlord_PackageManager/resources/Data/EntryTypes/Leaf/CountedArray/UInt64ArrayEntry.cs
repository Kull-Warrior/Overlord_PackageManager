using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class UInt64CountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<ulong>(id, relOffset)
    {
        protected override ulong ReadValue(BinaryReader reader) => reader.ReadUInt64();

        protected override void WriteValue(BinaryWriter writer, ulong value) => writer.Write(value);

        protected override int ElementSize => sizeof(ulong);
    }
}
