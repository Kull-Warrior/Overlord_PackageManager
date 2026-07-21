using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public abstract class RawListEntry<T>(uint id, uint relOffset) : ValueEntry<List<T>>(id, relOffset)
    {
        protected abstract int ElementSize { get; }

        protected abstract T ReadValue(BinaryReader reader);

        protected abstract void WriteValue(BinaryWriter writer, T value);

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            int count = (int)(PayloadLength / ElementSize);

            Value = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                Value.Add(ReadValue(reader));
            }
        }

        public override long GetPayloadSize()
        {
            return (Value?.Count ?? 0) * (long)ElementSize;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (var value in Value)
            {
                WriteValue(writer, value);
            }
        }
    }
}