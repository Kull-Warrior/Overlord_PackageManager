using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für FloatEditor.xaml
    /// </summary>
    public partial class FloatEditor : UserControl, IValueEditor
    {
        public FloatEditor()
        {
            InitializeComponent();
        }

        public FloatEditor(ObservableValue<float> value) : this()
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