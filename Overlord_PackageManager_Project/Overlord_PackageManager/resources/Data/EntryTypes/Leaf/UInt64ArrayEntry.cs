using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public class UInt64ArrayEntry(uint id, uint relOffset) : ValueEntry<ulong[]>(id, relOffset)
    {
        public uint NumberOfValues;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            NumberOfValues = reader.ReadUInt32();
            Value = new ulong[NumberOfValues];
            
            for (int i = 0; i < NumberOfValues; i++)
            {
                Value[i] = reader.ReadUInt64();
            }
        }

        public override long GetPayloadSize()
        {
            long totalSize = sizeof(uint);

            if (Value != null)
            {
                totalSize += sizeof(ulong) * Value.Length;
            }

            return totalSize;
        }


        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            writer.Write((uint)Value.Length);

            foreach (var number in Value)
            {
                writer.Write(number);
            }
        }
    }
}
