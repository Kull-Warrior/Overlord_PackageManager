using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    class Int32Entry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public uint varInt;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelOffset;
            varInt = reader.ReadUInt32();
        }
    }
}
