using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    class SingleByteEntry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        byte varByte;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varByte = reader.ReadByte();
        }
    }
}
