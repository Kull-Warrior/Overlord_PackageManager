using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList
{
    public class CountedListEntry<T>(uint id, uint relOffset, BinaryType<T> binaryType) : ValueEntry<List<T>>(id, relOffset)
    {
        protected BinaryType<T> BinaryType { get; } = binaryType;

        public int Count => Value?.Count ?? 0;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            uint count = reader.ReadUInt32();

            Value = new List<T>((int)count);

            for (int i = 0; i < count; i++)
            {
                Value.Add(BinaryType.Read(reader));
            }
        }

        public override long GetPayloadSize()
        {
            return sizeof(uint) + Count * (long)BinaryType.Size;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            writer.Write((uint)Count);

            foreach (var value in Value)
            {
                BinaryType.Write(writer, value);
            }
        }
    }
}