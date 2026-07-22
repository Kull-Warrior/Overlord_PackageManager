using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class CharCountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<char>(id, relOffset)
    {
        protected override int ElementSize => sizeof(char);

        protected override char ReadValue(BinaryReader reader) => reader.ReadChar();

        protected override void WriteValue(BinaryWriter writer, char value) => writer.Write(value);
    }
}