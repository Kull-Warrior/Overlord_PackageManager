using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class BoolEntry(uint id, uint relOffset) : ValueEntry<bool>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadBoolean();
        }

        public override long GetPayloadSize()
        {
            return sizeof(bool);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            writer.Write(Value);
        }
    }
}
