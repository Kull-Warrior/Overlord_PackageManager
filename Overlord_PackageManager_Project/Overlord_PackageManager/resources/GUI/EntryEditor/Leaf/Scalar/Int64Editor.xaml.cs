using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        public Int64Editor(ObservableValue<long> value) : this()
        {
            BindToLong(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<long> value)
            {
                BindToLong(value);
            }
        }

        private void BindToLong(ObservableValue<long> value)
        {
            DataContext = value;
        }
    }
}