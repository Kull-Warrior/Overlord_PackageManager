using System.IO;
using Overlord_PackageManager.resources.Data.Generic;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public abstract class ScalarEntry<T>(uint id, uint relOffset) : ValueEntry<T>(id, relOffset)
    {
        protected abstract T ReadValue(BinaryReader reader);
        protected abstract void WriteValue(BinaryWriter writer, T value);
        protected abstract int ElementSize { get; }

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = ReadValue(reader);
        }

        public override long GetPayloadSize()
        {
            return ElementSize;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            WriteValue(writer, Value);
        }
    }
}