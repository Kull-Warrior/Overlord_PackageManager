using Overlord_PackageManager.resources.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.EntryTypes.BaseTypes
{
    public class Int32Entry(uint id, uint relOffset) : Entry(id, relOffset)
    {
        public uint Value;

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadUInt32();
        }
    }
}
