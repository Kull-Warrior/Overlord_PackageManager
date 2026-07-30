using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        public BoolEditor(ObservableValue<bool> value) : this()
        {
            BindToBool(value);
        }

        public string Label
        {
            get => CheckBox.Content?.ToString() ?? "";
            set => CheckBox.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<bool> value)
            {
                BindToBool(value);
            }
        }

        private void BindToBool(ObservableValue<bool> value)
        {
            DataContext = value;
        }
    }
}