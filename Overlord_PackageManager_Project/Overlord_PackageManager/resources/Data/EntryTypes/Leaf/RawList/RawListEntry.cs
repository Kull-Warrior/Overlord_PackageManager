using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class RawListEntry<T>(uint id, uint relOffset, BinaryType<T> binaryType) : ValueEntry<List<T>>(id, relOffset)
    {
        protected BinaryType<T> BinaryType { get; } = binaryType;

        protected virtual bool IsCounted => false;

        protected virtual string CollectionSuffix => " List";

        public override string DisplayName => $"{(IsCounted ? "counted " : "")}{BinaryType.DisplayName}{CollectionSuffix}";

        protected override List<T> ReadValue(BinaryReader reader)
        {
            int count = (int)(PayloadLength / BinaryType.Size);

            List<T> values = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                values.Add(BinaryType.Read(reader));
            }

            return values;
        }

        public override long GetPayloadSize()
        {
            return (Value?.Count ?? 0) * (long)BinaryType.Size;
        }

        protected override void WriteValue(BinaryWriter writer, List<T> value)
        {
            foreach (T item in value)
            {
                BinaryType.Write(writer, item);
            }
        }
    }
}