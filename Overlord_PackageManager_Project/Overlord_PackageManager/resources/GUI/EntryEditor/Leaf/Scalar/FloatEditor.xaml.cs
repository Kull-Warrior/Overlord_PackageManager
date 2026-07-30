using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        public FloatEditor(ObservableValue<float> value) : this()
        {
            BindToFloat(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<float> value)
            {
                BindToFloat(value);
            }
        }

        private void BindToFloat(ObservableValue<float> value)
        {
            DataContext = value;
        }
    }
}