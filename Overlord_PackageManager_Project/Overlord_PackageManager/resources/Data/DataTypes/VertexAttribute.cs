namespace Overlord_PackageManager.resources.Data.DataTypes
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
        TangentQuat,
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
            0x0B => VertexAttributeSemantic.BlendIndices,
            0x0A => VertexAttributeSemantic.BlendWeights,
            0x09 => VertexAttributeSemantic.TangentQuat,
            _ => VertexAttributeSemantic.Unknown
        };

        public int ByteSize => Flags switch
        {
            1 => 8,     // FLOAT2
            2 => 12,    // FLOAT3
            3 => 16,    // FLOAT4
            4 => 1,     // UBYTE
            7 => 1,     // UBYTE (or normalised UBYTE)
            15 => 4,    // 4 bytes (unclear format, maybe UBYTE4)
            _ => throw new NotSupportedException($"Unknown format flag: {Flags}")
        };
    }
}