using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray
{
    public class StringCountedArrayEntry(uint id, uint relOffset) : CountedArrayEntry<StringLine>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override StringLine ReadValue(BinaryReader reader)
        {
            uint Length = reader.ReadUInt32();
            return new StringLine(Length, Encoding.ASCII.GetString(reader.ReadBytes((int)Length)));
        }

        public override long GetPayloadSize()
        {
            long totalSize = sizeof(uint);

            if (Value != null)
            {
                foreach (var line in Value)
                {
                    totalSize += sizeof(uint) + line.Length;
                }
            }

            return totalSize;
        }

        protected override void WriteValue(BinaryWriter writer, StringLine value)
        {
            writer.Write(value.Length);
            writer.Write(Encoding.ASCII.GetBytes(value.Text));
        }
    }
}