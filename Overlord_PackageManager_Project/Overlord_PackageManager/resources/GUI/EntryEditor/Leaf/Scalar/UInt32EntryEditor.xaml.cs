using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für UInt32Editor.xaml
    /// </summary>
    public partial class UInt32Editor : UserControl
    {
        public UInt32Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public UInt32Editor(ObservableValue<uint> value) : this()
        {
            BindToUInt32(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<uint> uintValue)
            {
                BindToUInt32(uintValue);
            }
        }

        private void BindToUInt32(ObservableValue<uint> uintValue)
        {
            // Bind the UInt32Editor to the ObservableValue<uint>
            DataContext = uintValue;
        }
    }
}