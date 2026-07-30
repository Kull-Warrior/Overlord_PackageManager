using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für Int32Editor.xaml
    /// </summary>
    public partial class Int32Editor : UserControl
    {
        public Int32Editor()
        {
            InitializeComponent();
        }

        public Int32Editor(ObservableValue<int> value) : this()
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