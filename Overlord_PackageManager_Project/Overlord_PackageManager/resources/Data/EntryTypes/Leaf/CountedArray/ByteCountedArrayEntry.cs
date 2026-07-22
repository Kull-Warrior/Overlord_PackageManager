using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class ByteCountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<byte>(id, relOffset)
    {
        protected override int ElementSize => sizeof(byte);

        protected override byte ReadValue(BinaryReader reader) => reader.ReadByte();

        protected override void WriteValue(BinaryWriter writer, byte value) => writer.Write(value);
    }
}