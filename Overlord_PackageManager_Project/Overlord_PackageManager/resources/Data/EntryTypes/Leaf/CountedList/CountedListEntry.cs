using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class CountedListEntry<T>(uint id, uint relOffset, BinaryType<T> binaryType) : ValueEntry<List<T>>(id, relOffset)
    {
        protected BinaryType<T> BinaryType { get; } = binaryType;

        protected virtual bool IsCounted => true;

        protected virtual string CollectionSuffix => " List";

        public override string DisplayName => $"{(IsCounted ? "counted " : "")}{BinaryType.DisplayName}{CollectionSuffix}";

        public int Count => Value?.Count ?? 0;

        protected override List<T> ReadValue(BinaryReader reader)
        {
            uint count = reader.ReadUInt32();

            List<T> values = new((int)count);

            for (int i = 0; i < count; i++)
            {
                values.Add(BinaryType.Read(reader));
            }

            return values;
        }

        public override long GetPayloadSize()
        {
            return sizeof(uint) + Count * (long)BinaryType.Size;
        }

        protected override void WriteValue(BinaryWriter writer, List<T> value)
        {
            writer.Write((uint)value.Count);

            foreach (T item in value)
            {
                BinaryType.Write(writer, item);
            }
        }
    }
}