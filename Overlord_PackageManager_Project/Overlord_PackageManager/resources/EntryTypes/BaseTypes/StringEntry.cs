using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    class StringEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        uint stringLength;
        public string varString;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            stringLength = reader.ReadUInt32();
            varString = Encoding.ASCII.GetString(reader.ReadBytes((int)stringLength));
        }

        public void Read(BinaryReader reader, long origin, uint length)
        {
            reader.BaseStream.Position = origin + RelOffset;
            stringLength = length;
            varString = Encoding.ASCII.GetString(reader.ReadBytes((int)stringLength));
        }
    }
}
