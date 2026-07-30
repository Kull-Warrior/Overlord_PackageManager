using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für UInt64Editor.xaml
    /// </summary>
    public partial class UInt64Editor : UserControl
    {
        public UInt64Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public UInt64Editor(ObservableValue<ulong> value) : this()
        {
            BindToUInt64(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<ulong> ulongValue)
            {
                BindToUInt64(ulongValue);
            }
        }

        private void BindToUInt64(ObservableValue<ulong> ulongValue)
        {
            // Bind the UInt64Editor to the ObservableValue<ulong>
            DataContext = ulongValue;
        }
    }
}