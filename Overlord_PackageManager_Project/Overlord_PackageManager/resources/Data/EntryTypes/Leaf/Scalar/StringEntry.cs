using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class StringEntry(uint id, uint relOffset) : ScalarEntry<string>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override string ReadValue(BinaryReader reader)
        {
            uint Length = reader.ReadUInt32();
            return Encoding.ASCII.GetString(reader.ReadBytes((int)Length));
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

        protected override void WriteValue(BinaryWriter writer, string value)
        {
            writer.Write((uint)value.Length);
            writer.Write(Encoding.ASCII.GetBytes(value));
        }
    }
}
