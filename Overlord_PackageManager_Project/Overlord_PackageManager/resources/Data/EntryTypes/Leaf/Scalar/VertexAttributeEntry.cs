using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar
{
    public class VertexAttributeEntry(uint id, uint relOffset) : ScalarEntry<VertexAttribute>(id, relOffset)
    {
        protected override int ElementSize => sizeof(uint);

        protected override VertexAttribute ReadValue(BinaryReader reader)
        {
            uint descriptor = reader.ReadUInt32();
            return new VertexAttribute(descriptor);
        }

        protected override void WriteValue(BinaryWriter writer, VertexAttribute value) => writer.Write(value.RawDescriptor);
    }
}