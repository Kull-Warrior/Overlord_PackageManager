using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class CharEntry(uint id, uint relOffset) : ScalarEntry<char>(id, relOffset)
    {
        protected override int ElementSize => 1;

        protected override char ReadValue(BinaryReader reader) => reader.ReadChar();

        protected override void WriteValue(BinaryWriter writer, char value) => writer.Write(value);
    }
}