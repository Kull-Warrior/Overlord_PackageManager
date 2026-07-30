using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class ByteEditor : UserControl
    {
        public ByteEditor()
        {
            InitializeComponent();
        }

        public ByteEditor(ObservableValue<byte> value) : this()
        {
            DataContext = value;
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }
    }
}