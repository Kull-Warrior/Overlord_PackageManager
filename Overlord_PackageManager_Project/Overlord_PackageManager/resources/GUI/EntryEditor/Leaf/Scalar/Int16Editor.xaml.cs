using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class Int16Editor : UserControl
    {
        public Int16Editor()
        {
            InitializeComponent();
        }

        public Int16Editor(ObservableValue<short> value) : this()
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