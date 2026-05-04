using Overlord_PackageManager.resources.Data.Generic;
using System.IO;

namespace Overlord_PackageManager.resources.Data.EntryTypes.Leaf
{
    public enum VertexAttributeSemantic
    {
        Position,
        Normal,
        TexCoord,
        Color,
        BlendIndices,
        BlendWeights,
        Tangent,
        Binormal,
        TangentSign,
        Unknown
    }

    public record VertexAttribute(uint RawDescriptor)
    {
        public byte Type => (byte)(RawDescriptor & 0xFF);
        public byte Index => (byte)((RawDescriptor >> 8) & 0xFF);
        public byte SemanticByte => (byte)((RawDescriptor >> 16) & 0xFF);
        public byte Flags => (byte)((RawDescriptor >> 24) & 0xFF);

        public VertexAttributeSemantic Semantic => SemanticByte switch
        {
            0x01 => VertexAttributeSemantic.Position,
            0x04 => VertexAttributeSemantic.Normal,
            0x05 => VertexAttributeSemantic.TexCoord,
            0x0B => VertexAttributeSemantic.Color,
            0x0A => VertexAttributeSemantic.Tangent,
            0x09 => VertexAttributeSemantic.TangentSign,
            _ => VertexAttributeSemantic.Unknown
        };
        public int ByteSize => Flags switch // How many bytes this attribute takes up in the vertex buffer
        {
            1 => 8,
            2 => 12,
            3 => 16,
            4 => 1,
            7 => 1,
            15 => 4,
            _ => throw new NotSupportedException($"Unknown flag: {Flags}")
        };
    }

    class VertexDeclarationEntry(uint id, uint relOffset): ValueEntry<List<VertexAttribute>>(id, relOffset)
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

            int attributeCount = Value.Count;
            int sizePerAttribute = sizeof(uint);
            long totalSize = attributeCount * sizePerAttribute;

            return totalSize;
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
