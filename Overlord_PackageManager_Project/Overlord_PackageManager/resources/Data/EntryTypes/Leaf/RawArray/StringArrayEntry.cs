using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class StringArrayEntry(uint id, uint relOffset) : RawArrayEntry<string>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override string ReadValue(BinaryReader reader)
        {
            uint Length = reader.ReadUInt32();
            return Encoding.ASCII.GetString(reader.ReadBytes((int)Length));
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

        protected override void WriteValue(BinaryWriter writer, string value)
        {
            writer.Write((uint)value.Length);
            writer.Write(Encoding.ASCII.GetBytes(value));
        }
    }
}