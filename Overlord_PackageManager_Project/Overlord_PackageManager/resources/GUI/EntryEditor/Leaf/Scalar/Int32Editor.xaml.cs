using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        public Int32Editor(ObservableValue<int> value) : this()
        {
            BindToInt(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<int> value)
            {
                BindToInt(value);
            }
        }

        private void BindToInt(ObservableValue<int> value)
        {
            DataContext = value;
        }
    }
}