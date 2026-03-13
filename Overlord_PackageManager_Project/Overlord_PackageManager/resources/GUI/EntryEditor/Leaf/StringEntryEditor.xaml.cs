using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    /// <summary>
    /// Interaktionslogik für StringEntryEditor.xaml
    /// </summary>
    public partial class StringEntryEditor : UserControl
    {
        private readonly StringEntry _entry;

        public StringEntryEditor(StringEntry entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = entry.Value;
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            _entry.Value = ValueBox.Text;
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }
    }
}