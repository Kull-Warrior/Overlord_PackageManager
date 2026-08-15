using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class RawArrayEntry<T>(uint id, uint relOffset, BinaryType<T> binaryType) : ValueEntry<T[]>(id, relOffset)
    {
        protected BinaryType<T> BinaryType { get; } = binaryType;

        protected virtual bool IsCounted => false;

        protected virtual string CollectionSuffix => "[]";

        public override string DisplayName => $"{(IsCounted ? "counted " : "")}{BinaryType.DisplayName}{CollectionSuffix}";

        protected override T[] ReadValue(BinaryReader reader)
        {
            int count = (int)(PayloadLength / BinaryType.Size);

            T[] values = new T[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = BinaryType.Read(reader);
            }

            return values;
        }

        public override long GetPayloadSize()
        {
            return (Value?.Length ?? 0) * (long)BinaryType.Size;
        }

        protected override void WriteValue(BinaryWriter writer, T[] value)
        {
            foreach (T item in value)
            {
                BinaryType.Write(writer, item);
            }
        }
    }
}