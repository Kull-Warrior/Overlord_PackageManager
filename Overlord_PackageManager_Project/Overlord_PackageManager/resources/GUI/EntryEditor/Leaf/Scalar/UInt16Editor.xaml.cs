using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class UInt16Editor : UserControl
    {
        public UInt16Editor()
        {
            InitializeComponent();
        }

        public UInt16Editor(ObservableValue<ushort> value) : this()
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