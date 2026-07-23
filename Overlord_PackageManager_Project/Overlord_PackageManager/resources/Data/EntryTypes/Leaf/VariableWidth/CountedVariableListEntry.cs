using System.IO;
using Overlord_PackageManager.resources.Data.Generic;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth
{
    public abstract class CountedVariableListEntry<T>(uint id, uint relOffset) : ValueEntry<List<T>>(id, relOffset)
    {
        protected abstract T ReadValue(BinaryReader reader);
        protected abstract void WriteValue(BinaryWriter writer, T value);
        protected abstract long GetValuePayloadSize(T value);

        public int Count => Value?.Count ?? 0;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            int count = checked((int)reader.ReadUInt32());
            Value = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                Value.Add(ReadValue(reader));
            }
        }

        public override long GetPayloadSize()
        {
            long size = sizeof(uint);

            if (Value is not null)
            {
                foreach (T value in Value)
                {
                    size += GetValuePayloadSize(value);
                }
            }

            return size;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            List<T> values = Value ?? new List<T>();
            writer.Write((uint)values.Count);

            foreach (T value in values)
            {
                WriteValue(writer, value);
            }
        }
    }
}