using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class CharCountedListEntry(uint id, uint relOffset) : CountedListEntry<char>(id, relOffset)
    {
        protected override int ElementSize => sizeof(char);

        protected override char ReadValue(BinaryReader reader) => reader.ReadChar();

        protected override void WriteValue(BinaryWriter writer, char value) => writer.Write(value);
    }
}