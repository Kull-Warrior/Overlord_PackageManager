using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    public partial class UInt16Editor : UserControl
    {
        public UInt16Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public UInt16Editor(ObservableValue<ushort> value) : this()
        {
            BindToUInt16(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<ushort> ushortValue)
            {
                BindToUInt16(ushortValue);
            }
        }

        private void BindToUInt16(ObservableValue<ushort> ushortValue)
        {
            // Bind the UInt16Editor to the ObservableValue<ushort>
            DataContext = ushortValue;
        }
    }
}