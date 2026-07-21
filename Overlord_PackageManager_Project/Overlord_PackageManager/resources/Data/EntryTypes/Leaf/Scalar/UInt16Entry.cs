using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class UInt16Entry(uint id, uint relOffset) : ScalarEntry<ushort>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ushort);

        protected override ushort ReadValue(BinaryReader reader) => reader.ReadUInt16();

        protected override void WriteValue(BinaryWriter writer, ushort value) => writer.Write(value);
    }
}