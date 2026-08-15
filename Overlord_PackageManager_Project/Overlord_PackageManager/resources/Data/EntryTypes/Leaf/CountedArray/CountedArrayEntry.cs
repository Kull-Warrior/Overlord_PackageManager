using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class CountedArrayEntry<T>(uint id, uint relOffset, BinaryType<T> binaryType) : ValueEntry<T[]>(id, relOffset)
    {
        protected BinaryType<T> BinaryType { get; } = binaryType;

        protected virtual bool IsCounted => true;

        protected virtual string CollectionSuffix => "[]";

        public override string DisplayName => $"{(IsCounted ? "counted " : "")}{BinaryType.DisplayName}{CollectionSuffix}";

        public int Count => Value?.Length ?? 0;

        protected override T[] ReadValue(BinaryReader reader)
        {
            uint count = reader.ReadUInt32();

            T[] values = new T[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = BinaryType.Read(reader);
            }

            return values;
        }

        public override long GetPayloadSize()
        {
            return sizeof(uint) + Count * (long)BinaryType.Size;
        }

        protected override void WriteValue(BinaryWriter writer, T[] value)
        {
            writer.Write((uint)value.Length);

            foreach (T item in value)
            {
                BinaryType.Write(writer, item);
            }
        }
    }
}