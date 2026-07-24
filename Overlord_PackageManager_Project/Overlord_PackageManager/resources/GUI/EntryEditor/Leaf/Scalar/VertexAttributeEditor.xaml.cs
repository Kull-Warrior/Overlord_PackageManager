using Overlord_PackageManager.resources.Data.DataTypes;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class VertexAttributeEditor : UserControl
    {
        private bool _updating;

        public event EventHandler<VertexAttribute>? ValueChanged;

        public VertexAttribute Value { get; private set; }

        private static readonly byte[] AllowedSizes = [1, 4, 8, 12, 16];

        public VertexAttributeEditor(): this(new VertexAttribute(0))
        {

        }

        public VertexAttributeEditor(VertexAttribute value)
        {
            InitializeComponent();

            if (value != null)
            {
                Value = value;
            }
            else
            {
                Value = new VertexAttribute(0);
            }

            SemanticBox.ItemsSource = Enum.GetValues(typeof(VertexAttributeSemantic)).Cast<VertexAttributeSemantic>();

            SizeBox.ItemsSource = AllowedSizes;

            LoadFromValue();
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void LoadFromValue()
        {
            _updating = true;

            TypeBox.Text = Value.Type.ToString();
            IndexBox.Text = Value.Index.ToString();

            VertexAttributeSemantic semantic = SemanticFromByte(Value.SemanticByte);
            if (semantic == VertexAttributeSemantic.Unknown)
            {
                SemanticBox.SelectedItem = VertexAttributeSemantic.Position;
            }
            else
            {
                SemanticBox.SelectedItem = semantic;
            }

            byte size = ByteSizeFromFlags(Value.Flags);
            SizeBox.SelectedItem = AllowedSizes.Contains(size) ? size : (byte)12;

            RawBlock.Text = $"Raw: 0x{Value.RawDescriptor:X8}";

            _updating = false;
        }

        private void AnyTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating)
            {
                return;
            }
            UpdateValue();
        }

        private void AnySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_updating)
            {
                return;
            }
            UpdateValue();
        }

        private void UpdateValue()
        {
            byte type = ParseByte(TypeBox.Text, 0);
            byte index = ParseByte(IndexBox.Text, 0);

            VertexAttributeSemantic semantic;
            if (SemanticBox.SelectedItem is VertexAttributeSemantic s)
            {
                semantic = s;
            }
            else
            {
                semantic = VertexAttributeSemantic.Position;
            }

            byte semanticByte = SemanticToByte(semantic);

            byte byteSize;
            if (SizeBox.SelectedItem is byte b)
            {
                byteSize = b;
            }
            else
            {
                byteSize = (byte)12;
            }


            byte flags = FlagsFromByteSize(byteSize);

            uint raw =
                (uint)type |
                ((uint)index << 8) |
                ((uint)semanticByte << 16) |
                ((uint)flags << 24);

            Value = new VertexAttribute(raw);

            RawBlock.Text = $"Raw: 0x{Value.RawDescriptor:X8}";
            ValueChanged?.Invoke(this, Value);
        }

        private static byte ParseByte(string text, byte fallback)
        {
            byte result;
            if (byte.TryParse(text, out result))
            {
                return result;
            }
            else
            {
                return fallback;
            }

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