using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class ByteEditor : UserControl
    {
        public ByteEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public ByteEditor(ObservableValue<byte> value) : this()
        {
            BindToByte(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<byte> value)
            {
                BindToByte(value);
            }
        }

        private void BindToByte(ObservableValue<byte> value)
        {
            DataContext = value;
        }
    }
}