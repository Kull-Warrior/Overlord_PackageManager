using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class UInt16CountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<ushort>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ushort);

        protected override ushort ReadValue(BinaryReader reader) => reader.ReadUInt16();

        protected override void WriteValue(BinaryWriter writer, ushort value) => writer.Write(value);
    }
}