using System.IO;
using Overlord_PackageManager.resources.Data.Generic;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth
{
    public abstract class CountedVariableListEntry<T>(uint id, uint relOffset) : ValueEntry<List<T>>(id, relOffset)
    {
        protected abstract T ReadElement(BinaryReader reader);
        protected abstract void WriteElement(BinaryWriter writer, T value);
        protected abstract long GetValuePayloadSize(T value);

        public int Count => Value?.Count ?? 0;

        protected override List<T> ReadValue(BinaryReader reader)
        {
            int count = checked((int)reader.ReadUInt32());
            List<T> values = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                values.Add(ReadElement(reader));
            }

            return values;
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

        protected override void WriteValue(BinaryWriter writer, List<T> value)
        {
            List<T> values = value ?? new List<T>();
            writer.Write((uint)values.Count);

            foreach (T item in values)
            {
                WriteElement(writer, item);
            }
        }
    }
}