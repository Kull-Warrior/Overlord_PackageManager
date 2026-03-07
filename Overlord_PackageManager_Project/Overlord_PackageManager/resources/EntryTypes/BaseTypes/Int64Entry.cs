using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class Int64Entry : Entry
    {
        public ulong Value;

        public Int64Entry(uint id, uint relOffset) : base(id, relOffset)
        {

        }
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadUInt64();
        }
    }
}
