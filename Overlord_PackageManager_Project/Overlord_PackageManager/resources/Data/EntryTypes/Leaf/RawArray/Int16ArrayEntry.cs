using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class Int16ArrayEntry(uint id, uint relOffset) : ValueEntry<short[]>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new short[PayloadLength / sizeof(short)];

            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = reader.ReadInt16();
            }
        }

        public override long GetPayloadSize()
        {
            long totalSize = sizeof(short) * Value.Length;

            return totalSize;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (short number in Value)
            {
                writer.Write(number);
            }
        }
    }
}
