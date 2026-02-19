using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.EntryEditor
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
            ValueBox.Text = entry.varString;
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            _entry.varString = ValueBox.Text;
        }
    }
}
