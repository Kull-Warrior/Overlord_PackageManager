using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class ScalarEntry<T> : ValueEntry<T>
    {
        private readonly BinaryType<T> _binaryType;

        public ScalarEntry(uint id, uint relOffset, BinaryType<T> binaryType) : base(id, relOffset)
        {
            _binaryType = binaryType;
        }

        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;
            Value = _binaryType.Read(reader);
        }

        public override long GetPayloadSize()
        {
            return _binaryType.Size;
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;
            _binaryType.Write(writer, Value);
        }
    }
}