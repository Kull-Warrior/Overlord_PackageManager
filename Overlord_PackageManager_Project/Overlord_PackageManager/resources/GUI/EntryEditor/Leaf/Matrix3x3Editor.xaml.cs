using Overlord_PackageManager.resources.Data.DataTypes;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class Matrix3x3Editor : UserControl
    {
        private bool _updating;

        public event EventHandler<Matrix3x3>? ValueChanged;

        public Matrix3x3 Value { get; set; }

        public Matrix3x3Editor()
            : this(new Matrix3x3(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1))
        {
        }

        public Matrix3x3Editor(Matrix3x3 value)
        {
            InitializeComponent();

            Value = value;

            LoadFromValue();
        }

        public string Label
        {
            get => LabelBlock.Text;
            set
            {
                LabelBlock.Text = value;
                LabelBlock.Visibility = string.IsNullOrWhiteSpace(value) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }
        }

        public void LoadFromValue()
        {
            _updating = true;

            M11Box.Text = Value.M11.ToString();
            M12Box.Text = Value.M12.ToString();
            M13Box.Text = Value.M13.ToString();

            M21Box.Text = Value.M21.ToString();
            M22Box.Text = Value.M22.ToString();
            M23Box.Text = Value.M23.ToString();

            M31Box.Text = Value.M31.ToString();
            M32Box.Text = Value.M32.ToString();
            M33Box.Text = Value.M33.ToString();

            _updating = false;
        }

        private void AnyTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Value = new Matrix3x3(
                ParseFloat(M11Box.Text),
                ParseFloat(M12Box.Text),
                ParseFloat(M13Box.Text),

                ParseFloat(M21Box.Text),
                ParseFloat(M22Box.Text),
                ParseFloat(M23Box.Text),

                ParseFloat(M31Box.Text),
                ParseFloat(M32Box.Text),
                ParseFloat(M33Box.Text));

            ValueChanged?.Invoke(this, Value);
        }

        private static float ParseFloat(string text)
        {
            return float.TryParse(text, out float value)
                ? value
                : 0f;
        }
    }
}