using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public record StringLine(uint Length, string Text);

    public class StringListEntry(uint id, uint relOffset) : ValueEntry<List<StringLine>>(id, relOffset)
    {
        public uint NumberOfLines { get; set; }
        public List<uint> LineLengths { get; set; } = new List<uint>();

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            NumberOfLines = reader.ReadUInt32();
            Value = new List<StringLine>((int)NumberOfLines);

            for (uint i = 0; i < NumberOfLines; i++)
            {
                uint length = reader.ReadUInt32();
                byte[] bytes = reader.ReadBytes((int)length);
                string str = Encoding.ASCII.GetString(bytes);

                Value.Add(new StringLine(length, str));
            }
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

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            writer.Write((uint)Value.Count);

            foreach (var line in Value)
            {
                writer.Write(line.Length);
                byte[] bytes = Encoding.ASCII.GetBytes(line.Text);
                writer.Write(bytes);
            }
        }
    }
}
