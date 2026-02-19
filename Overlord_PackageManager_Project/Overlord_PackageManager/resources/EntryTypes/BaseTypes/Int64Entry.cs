using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class Int64Entry : Entry
    {
        public ulong varInt;

        public Int64Entry(uint id, uint relOffset) : base(id, relOffset)
        {

        }
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varInt = reader.ReadUInt64();
        }
    }
}
