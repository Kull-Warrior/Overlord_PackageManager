using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class UInt64Entry(uint id, uint relOffset) : ScalarEntry<ulong>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ulong);

        protected override ulong ReadValue(BinaryReader reader) => reader.ReadUInt64();

        protected override void WriteValue(BinaryWriter writer, ulong value) => writer.Write(value);
    }
}