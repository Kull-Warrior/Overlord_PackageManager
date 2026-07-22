using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class UInt32CountedListEntry(uint id, uint relOffset) : CountedListEntry<uint>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override uint ReadValue(BinaryReader reader) => reader.ReadUInt32();

        protected override void WriteValue(BinaryWriter writer, uint value) => writer.Write(value);
    }
}