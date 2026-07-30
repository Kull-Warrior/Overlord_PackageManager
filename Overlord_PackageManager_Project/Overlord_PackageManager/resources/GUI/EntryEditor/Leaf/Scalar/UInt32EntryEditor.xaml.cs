using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für UInt32Editor.xaml
    /// </summary>
    public partial class UInt32Editor : UserControl
    {
        public UInt32Editor()
        {
            InitializeComponent();
        }

        public UInt32Editor(ObservableValue<uint> value) : this()
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