using System.Numerics;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class Matrix4x4Editor : UserControl
    {
        private bool _updating;

        public event EventHandler<Matrix4x4>? ValueChanged;

        public Matrix4x4 Value { get; private set; }

        public Matrix4x4Editor() : this(Matrix4x4.Identity)
        {
        }

        public Matrix4x4Editor(Matrix4x4 value)
        {
            InitializeComponent();

            Value = value;

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

            M11Box.Text = Value.M11.ToString();
            M12Box.Text = Value.M12.ToString();
            M13Box.Text = Value.M13.ToString();
            M14Box.Text = Value.M14.ToString();

            M21Box.Text = Value.M21.ToString();
            M22Box.Text = Value.M22.ToString();
            M23Box.Text = Value.M23.ToString();
            M24Box.Text = Value.M24.ToString();

            M31Box.Text = Value.M31.ToString();
            M32Box.Text = Value.M32.ToString();
            M33Box.Text = Value.M33.ToString();
            M34Box.Text = Value.M34.ToString();

            M41Box.Text = Value.M41.ToString();
            M42Box.Text = Value.M42.ToString();
            M43Box.Text = Value.M43.ToString();
            M44Box.Text = Value.M44.ToString();

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

        private void UpdateValue()
        {
            Value = new Matrix4x4(
                ParseFloat(M11Box.Text), ParseFloat(M12Box.Text), ParseFloat(M13Box.Text), ParseFloat(M14Box.Text),
                ParseFloat(M21Box.Text), ParseFloat(M22Box.Text), ParseFloat(M23Box.Text), ParseFloat(M24Box.Text),
                ParseFloat(M31Box.Text), ParseFloat(M32Box.Text), ParseFloat(M33Box.Text), ParseFloat(M34Box.Text),
                ParseFloat(M41Box.Text), ParseFloat(M42Box.Text), ParseFloat(M43Box.Text), ParseFloat(M44Box.Text)
            );

            ValueChanged?.Invoke(this, Value);
        }

        private static float ParseFloat(string text)
        {
            return float.TryParse(text, out float value) ? value : 0f;
        }
    }
}