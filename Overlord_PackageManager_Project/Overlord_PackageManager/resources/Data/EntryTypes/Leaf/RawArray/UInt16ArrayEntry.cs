using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class UInt16ArrayEntry(uint id, uint relOffset) : ValueEntry<ushort[]>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = new ushort[PayloadLength / sizeof(ushort)];

            for (int i = 0; i < Value.Length; i++)
            {
                Value[i] = reader.ReadUInt16();
            }
        }

        public override long GetPayloadSize()
        {
            long totalSize = sizeof(ushort) * Value.Length;

            return totalSize;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (ushort number in Value)
            {
                writer.Write(number);
            }
        }
    }
}
