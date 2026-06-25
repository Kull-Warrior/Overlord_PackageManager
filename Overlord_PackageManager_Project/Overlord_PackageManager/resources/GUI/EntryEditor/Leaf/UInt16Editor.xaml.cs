using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.Leaf
{
    public partial class UInt16Editor : UserControl
    {
        public event EventHandler<ushort>? ValueChanged;

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(UInt16Editor),
                new PropertyMetadata(string.Empty, OnLabelChanged));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public ushort Value
        {
            get;
            private set;
        }

        public UInt16Editor()
        {
            InitializeComponent();
        }

        public UInt16Editor(ushort value)
            : this()
        {
            Value = value;
            ValueBox.Text = value.ToString();
        }

        private static void OnLabelChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((UInt16Editor)d).LabelText.Text = (string)e.NewValue;
        }

        private void ValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ushort.TryParse(ValueBox.Text, out ushort value))
            {
                Value = value;
                ValueChanged?.Invoke(this, value);
            }
        }
    }
}