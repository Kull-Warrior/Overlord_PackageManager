using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class DoubleEntry(uint id, uint relOffset) : ValueEntry<double>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = reader.ReadDouble();
        }

        public override long GetPayloadSize()
        {
            return sizeof(double);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            writer.Write(Value);
        }
    }
}
