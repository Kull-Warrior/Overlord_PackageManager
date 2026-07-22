using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public abstract class CountedListEntry<T>(uint id, uint relOffset) : ValueEntry<List<T>>(id, relOffset)
    {
        protected abstract T ReadValue(BinaryReader reader);
        protected abstract void WriteValue(BinaryWriter writer, T value);
        protected abstract int ElementSize { get; }

        public int Count => Value?.Count ?? 0;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            uint count = reader.ReadUInt32();

            Value = new List<T>((int)count);

            for (int i = 0; i < count; i++)
            {
                Value.Add(ReadValue(reader));
            }
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            writer.Write((uint)Count);

            foreach (var value in Value)
            {
                WriteValue(writer, value);
            }
        }

        public override long GetPayloadSize()
        {
            return sizeof(uint) + Count * (long)ElementSize;
        }
    }
}