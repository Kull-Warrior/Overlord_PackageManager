using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class StringListEntry(uint id, uint relOffset) : ValueEntry<List<string>>(id, relOffset)
    {
        public uint NumberOfLines { get; set; }
        public List<uint> LineLengths { get; set; } = new List<uint>();

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            NumberOfLines = reader.ReadUInt32();
            Value = new List<string>((int)NumberOfLines);

            for (uint i = 0; i < NumberOfLines; i++)
            {
                uint length = reader.ReadUInt32();
                LineLengths.Add(length);

                string str = Encoding.ASCII.GetString(reader.ReadBytes((int)length));
                Value.Add(str);
            }
        }
    }
}
