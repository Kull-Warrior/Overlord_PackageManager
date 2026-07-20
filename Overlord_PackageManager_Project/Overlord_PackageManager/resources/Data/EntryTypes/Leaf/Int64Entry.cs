using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public class Int64Entry(uint id, uint relOffset) : ValueEntry<long>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadInt64();
        }

        public override long GetPayloadSize()
        {
            return sizeof(long);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            writer.Write(Value);
        }
    }
}
