using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth
{
    public sealed class CharListCountedArrayEntry(uint id, uint relOffset) : CountedVariableListEntry<char[]>(id, relOffset)
    {
        protected override char[] ReadValue(BinaryReader reader)
        {
            int charCount = checked((int)reader.ReadUInt32());
            char[] characters = reader.ReadChars(charCount);

            if (characters.Length != charCount)
            {
                throw new EndOfStreamException();
            }

            return characters;
        }

        protected override void WriteValue(BinaryWriter writer, char[] value)
        {
            writer.Write((uint)value.Length);
            writer.Write(value);
        }

        protected override long GetValuePayloadSize(char[] value)
        {
            return sizeof(uint) + value.LongLength;
        }
    }
}