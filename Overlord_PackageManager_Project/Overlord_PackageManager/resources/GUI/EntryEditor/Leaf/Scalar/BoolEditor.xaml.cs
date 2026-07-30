using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für BoolEditor.xaml
    /// </summary>
    public partial class BoolEditor : UserControl, IValueEditor
    {
        public BoolEditor()
        {
            InitializeComponent();
        }

        public BoolEditor(ObservableValue<bool> value) : this()
        {
            DataContext = value;
        }

        public string Label
        {
            get => CheckBox.Content?.ToString() ?? "";
            set => CheckBox.Content = value;
        }
    }
}