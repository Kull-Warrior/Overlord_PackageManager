using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    /// <summary>
    /// Interaktionslogik für StringEntryEditor.xaml
    /// </summary>
    public partial class StringEntryEditor : UserControl
    {
        private readonly CharCountedArrayEntry _entry;

        public StringEntryEditor(CharCountedArrayEntry entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = new string(entry.Value);
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            _entry.Value = ValueBox.Text.ToCharArray();
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }
    }
}