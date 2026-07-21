using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class ByteEntry(uint id, uint relOffset) : ScalarEntry<byte>(id, relOffset)
    {
        protected override int ElementSize => sizeof(byte);

        protected override byte ReadValue(BinaryReader reader) => reader.ReadByte();

        protected override void WriteValue(BinaryWriter writer, byte value) => writer.Write(value);
    }
}