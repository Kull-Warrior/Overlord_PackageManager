using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    /// <summary>
    /// Observable wrapper for VertexAttribute for UI binding.
    /// </summary>
    public class ObservableVertexAttribute : ObservableComposite
    {
        private VertexAttribute _value;

        public ObservableValue<byte> Type { get; }
        public ObservableValue<byte> Index { get; }
        public ObservableValue<VertexAttributeSemantic> Semantic { get; }
        public ObservableValue<byte> ByteSize { get; }

        public VertexAttribute Value => _value;

        // Allowed sizes for the dropdown
        public static readonly byte[] AllowedSizes = [1, 4, 8, 12, 16];

        public ObservableVertexAttribute(VertexAttribute initial)
        {
            _value = initial;

            Type = new(initial.Type);
            Index = new(initial.Index);
            Semantic = new(SemanticFromByte(initial.SemanticByte));
            ByteSize = new(ByteSizeFromFlags(initial.Flags));

            Subscribe(
                Type,
                Index,
                Semantic,
                ByteSize);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObservableValue<byte>.Value) &&
                e.PropertyName != nameof(ObservableValue<VertexAttributeSemantic>.Value))
            {
                return;
            }

            byte semanticByte = SemanticToByte(Semantic.Value);
            byte flags = FlagsFromByteSize(ByteSize.Value);

            uint raw =
                (uint)Type.Value |
                ((uint)Index.Value << 8) |
                ((uint)semanticByte << 16) |
                ((uint)flags << 24);

            _value = new VertexAttribute(raw);

            OnPropertyChanged(nameof(Value));
        }

        private static byte SemanticToByte(VertexAttributeSemantic semantic)
        {
            return semantic switch
            {
                VertexAttributeSemantic.Position => 0x01,
                VertexAttributeSemantic.Normal => 0x04,
                VertexAttributeSemantic.TexCoord => 0x05,
                VertexAttributeSemantic.Color => 0x06,
                VertexAttributeSemantic.BlendWeights => 0x0A,
                VertexAttributeSemantic.BlendIndices => 0x0B,
                VertexAttributeSemantic.Tangent => 0x08,
                VertexAttributeSemantic.Binormal => 0x07,
                VertexAttributeSemantic.TangentSign => 0x0C,
                VertexAttributeSemantic.TangentQuat => 0x09,
                _ => 0x00
            };
        }

        private static VertexAttributeSemantic SemanticFromByte(byte semanticByte)
        {
            return semanticByte switch
            {
                0x01 => VertexAttributeSemantic.Position,
                0x04 => VertexAttributeSemantic.Normal,
                0x05 => VertexAttributeSemantic.TexCoord,
                0x06 => VertexAttributeSemantic.Color,
                0x0A => VertexAttributeSemantic.BlendWeights,
                0x0B => VertexAttributeSemantic.BlendIndices,
                0x08 => VertexAttributeSemantic.Tangent,
                0x07 => VertexAttributeSemantic.Binormal,
                0x0C => VertexAttributeSemantic.TangentSign,
                0x09 => VertexAttributeSemantic.TangentQuat,
                _ => VertexAttributeSemantic.Unknown
            };
        }

        private static byte FlagsFromByteSize(byte byteSize)
        {
            return byteSize switch
            {
                8 => 1,
                12 => 2,
                16 => 3,
                1 => 4,
                4 => 15,
                _ => 2 // default to FLOAT3 / 12 bytes
            };
        }

        private static byte ByteSizeFromFlags(byte flags)
        {
            return flags switch
            {
                1 => 8,
                2 => 12,
                3 => 16,
                4 => 1,
                7 => 1,
                15 => 4,
                _ => 12 // default to FLOAT3 / 12 bytes
            };
        }
    }
}