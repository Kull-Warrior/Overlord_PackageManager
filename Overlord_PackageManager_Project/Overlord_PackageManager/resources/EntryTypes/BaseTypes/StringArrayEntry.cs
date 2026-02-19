using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class StringArrayEntry : Entry
    {
        uint stringCount;
        List<uint> stringLength = new List<uint>();
        List<string> varString = new List<string>();

        public StringArrayEntry(uint id, uint relOffset) : base(id, relOffset)
        {

        }
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            stringCount = reader.ReadUInt32();
            for (uint i = 0; i < stringCount; i++)
            {
                uint length = reader.ReadUInt32();
                stringLength.Add(length);
                varString.Add(Encoding.ASCII.GetString(reader.ReadBytes((int)length)));
            }
        }
    }
}
