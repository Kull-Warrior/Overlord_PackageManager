using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für DoubleEditor.xaml
    /// </summary>
    public partial class DoubleEditor : UserControl
    {
        public DoubleEditor()
        {
            InitializeComponent();
        }

        public DoubleEditor(ObservableValue<double> value) : this()
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