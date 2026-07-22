using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class BoolCountedListEntry(uint id, uint relOffset) : CountedListEntry<bool>(id, relOffset)
    {
        protected override int ElementSize => sizeof(byte);

        protected override bool ReadValue(BinaryReader reader) => reader.ReadBoolean();

        protected override void WriteValue(BinaryWriter writer, bool value) => writer.Write(value);
    }
}