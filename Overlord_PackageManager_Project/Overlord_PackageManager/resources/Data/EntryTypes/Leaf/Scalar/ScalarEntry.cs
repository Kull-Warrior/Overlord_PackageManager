using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class ScalarEntry<T> : ValueEntry<T>
    {
        private readonly BinaryType<T> _binaryType;

        public override string DisplayName => _binaryType.DisplayName;

        public ScalarEntry(uint id, uint relOffset, BinaryType<T> binaryType) : base(id, relOffset)
        {
            _binaryType = binaryType;
        }

        protected override T ReadValue(BinaryReader reader)
        {
            return _binaryType.Read(reader);
        }

        public override long GetPayloadSize()
        {
            return _binaryType.Size;
        }

        protected override void WriteValue(BinaryWriter writer, T value)
        {
            _binaryType.Write(writer, value);
        }
    }
}