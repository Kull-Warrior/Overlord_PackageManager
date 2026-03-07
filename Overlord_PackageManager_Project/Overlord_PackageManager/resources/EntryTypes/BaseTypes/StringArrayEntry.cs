using Overlord_PackageManager.resources.Generic;
using System.IO;
using System.Text;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class StringArrayEntry : Entry
    {
        uint Count;
        List<uint> Lengths = new List<uint>();
        List<string> Values = new List<string>();

        public StringArrayEntry(uint id, uint relOffset) : base(id, relOffset)
        {

        }
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Count = reader.ReadUInt32();
            for (uint i = 0; i < Count; i++)
            {
                uint length = reader.ReadUInt32();
                Lengths.Add(length);
                Values.Add(Encoding.ASCII.GetString(reader.ReadBytes((int)length)));
            }
        }
    }
}
