using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth
{
    public sealed class CharListCountedArrayEntry(uint id, uint relOffset) : CountedVariableListEntry<char[]>(id, relOffset)
    {
        protected override char[] ReadElement(BinaryReader reader)
        {
            int charCount = checked((int)reader.ReadUInt32());

            char[] characters = new char[charCount];
            for (int i = 0; i < charCount; i++)
            {
                char c = BinaryTypes.Char.Read(reader);
                characters[i] = c;
            }

            if (characters.Length != charCount)
            {
                throw new EndOfStreamException();
            }

            return characters;
        }

        protected override void WriteElement(BinaryWriter writer, char[] value)
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