using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    class ByteEntry : Entry
    {
        byte varByte;

        public ByteEntry(uint id, uint relOffset) : base(id, relOffset)
        {

        }

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varByte = reader.ReadByte();
        }
    }
}
