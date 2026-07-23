using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class RawListEntry<T>(uint id, uint relOffset, BinaryType<T> binaryType) : ValueEntry<List<T>>(id, relOffset)
    {
        protected BinaryType<T> BinaryType { get; } = binaryType;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            int count = (int)(PayloadLength / BinaryType.Size);

            Value = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                Value.Add(BinaryType.Read(reader));
            }
        }

        public override long GetPayloadSize()
        {
            return (Value?.Count ?? 0) * (long)BinaryType.Size;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (var value in Value)
            {
                BinaryType.Write(writer, value);
            }
        }
    }
}