using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaktionslogik für CharEditor.xaml
    /// </summary>
    public partial class CharEditor : UserControl
    {
        public CharEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public CharEditor(ObservableValue<char> value) : this()
        {
            BindToChar(value);
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableValue<char> value)
            {
                BindToChar(value);
            }
        }

        private void BindToChar(ObservableValue<char> value)
        {
            DataContext = value;
        }
    }
}