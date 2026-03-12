using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class StringEntry(uint id, uint relOffset) : ValueEntry<string>(id, relOffset)
    {
        uint Length;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Length = reader.ReadUInt32();
            Value = Encoding.ASCII.GetString(reader.ReadBytes((int)Length));
        }

        public override long GetPayloadSize()
        {
            // Auch wenn Value null oder leer ist, existiert der Header (4 Bytes für die 0)
            long totalSize = sizeof(uint);

            if (Value != null)
            {
                totalSize += Value.Length;
            }

            return totalSize;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            writer.Write((uint)Value.Length);
            writer.Write(Encoding.ASCII.GetBytes(Value));
        }
    }
}
