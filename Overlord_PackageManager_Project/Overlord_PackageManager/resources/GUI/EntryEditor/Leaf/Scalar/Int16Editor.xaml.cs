using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class Int16Editor : UserControl
    {
        public Int16Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public Int16Editor(ObservableValue<short> value) : this()
        {
            BindToShort(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<short> value)
            {
                BindToShort(value);
            }
        }

        private void BindToShort(ObservableValue<short> value)
        {
            DataContext = value;
        }
    }
}