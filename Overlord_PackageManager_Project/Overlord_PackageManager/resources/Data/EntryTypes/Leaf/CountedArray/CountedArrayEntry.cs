using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public abstract class CountedArrayEntry<T>(uint id, uint relOffset) : ValueEntry<T[]>(id, relOffset)
    {
        protected abstract T ReadValue(BinaryReader reader);

        protected abstract void WriteValue(BinaryWriter writer, T value);

        protected abstract int ElementSize { get; }

        public int Count => Value?.Length ?? 0;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            uint count = reader.ReadUInt32();

            Value = new T[count];

            for (int i = 0; i < count; i++)
            {
                Value[i] = ReadValue(reader);
            }
        }

        public override long GetPayloadSize()
        {
            return sizeof(uint) + Count * (long)ElementSize;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            writer.Write((uint)Count);

            foreach (T? value in Value)
            {
                WriteValue(writer, value);
            }
        }
    }
}