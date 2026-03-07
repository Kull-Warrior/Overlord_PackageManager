using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class StringEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        uint Length;
        public string Value;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Length = reader.ReadUInt32();
            Value = Encoding.ASCII.GetString(reader.ReadBytes((int)Length));
        }

        public void Read(BinaryReader reader, long origin, uint length)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Length = length;
            Value = Encoding.ASCII.GetString(reader.ReadBytes((int)Length));
        }
    }
}
