using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class UInt16CountedListEntry(uint id, uint relOffset) : CountedListEntry<ushort>(id, relOffset)
    {
        protected override int ElementSize => sizeof(ushort);

        protected override ushort ReadValue(BinaryReader reader) => reader.ReadUInt16();

        protected override void WriteValue(BinaryWriter writer, ushort value) => writer.Write(value);
    }
}