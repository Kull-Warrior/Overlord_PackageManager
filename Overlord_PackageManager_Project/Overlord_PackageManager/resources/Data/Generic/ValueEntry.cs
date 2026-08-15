using System.IO;

namespace Overlord_PackageManager.resources.Data.Generic
{
    public abstract class ValueEntry<T>(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public T Value { get; set; }

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = ReadValue(reader);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            WriteValue(writer, Value);
        }

        public void ReadPayload(BinaryReader reader)
        {
            Value = ReadValue(reader);
        }

        public void WritePayload(BinaryWriter writer)
        {
            WriteValue(writer, Value);
        }

        protected abstract T ReadValue(BinaryReader reader);

        protected abstract void WriteValue(BinaryWriter writer, T value);
    }
}