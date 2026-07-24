using System.Numerics;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class Vector3Editor : UserControl
    {
        private bool _updating;

        public event EventHandler<Vector3>? ValueChanged;

        public Vector3 Value { get; set; }

        public Vector3Editor() : this(Vector3.Zero)
        {
        }

        public Vector3Editor(Vector3 value)
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

            XBox.Text = Value.X.ToString();
            YBox.Text = Value.Y.ToString();
            ZBox.Text = Value.Z.ToString();

            _updating = false;
        }

        private void AnyTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Value = new Vector3(
                ParseFloat(XBox.Text),
                ParseFloat(YBox.Text),
                ParseFloat(ZBox.Text));

            ValueChanged?.Invoke(this, Value);
        }

        private static float ParseFloat(string text)
        {
            return float.TryParse(text, out float value) ? value : 0f;
        }
    }
}