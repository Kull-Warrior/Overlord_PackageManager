using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für Int64Editor.xaml
    /// </summary>
    public partial class Int64Editor : UserControl
    {
        public Int64Editor()
        {
            InitializeComponent();
        }

        public Int64Editor(ObservableValue<long> value) : this()
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