using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public class Int32Entry(uint id, uint relOffset) : ValueEntry<int>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadInt32();
        }

        public override long GetPayloadSize()
        {
            return sizeof(int);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            writer.Write(Value);
        }
    }
}
