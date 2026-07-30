using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        public DoubleEditor(ObservableValue<double> value) : this()
        {
            BindToDouble(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<double> value)
            {
                BindToDouble(value);
            }
        }

        private void BindToDouble(ObservableValue<double> value)
        {
            DataContext = value;
        }
    }
}