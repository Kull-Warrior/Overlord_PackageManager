using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList
{
    public class VertexDeclarationEntry(uint id, uint relOffset) : ValueEntry<List<VertexAttribute>>(id, relOffset)
    {
        public override void Read(BinaryReader reader, long origin)
        {
            reader.BaseStream.Position = origin + RelativeOffset;

            int count = (int)(PayloadLength / 4);
            Value = new List<VertexAttribute>(count);

            for (int i = 0; i < count; i++)
            {
                uint descriptor = reader.ReadUInt32();
                Value.Add(new VertexAttribute(descriptor));
            }
        }

        public override long GetPayloadSize()
        {
            if (Value == null)
            {
                return 0;
            }

            return Value.Count * sizeof(uint);
        }

        public override void Write(BinaryWriter writer, long origin)
        {
            writer.BaseStream.Position = origin + RelativeOffset;

            foreach (VertexAttribute attribute in Value)
            {
                writer.Write(attribute.RawDescriptor);
            }
        }
    }
}
