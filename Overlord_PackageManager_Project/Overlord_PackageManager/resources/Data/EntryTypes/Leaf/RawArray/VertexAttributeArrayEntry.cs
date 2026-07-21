using Overlord_PackageManager.resources.Data.DataTypes;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray
{
    public class VertexAttributeArrayEntry(uint id, uint relOffset) : RawArrayEntry<VertexAttribute>(id, relOffset)
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