using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.EntryEditor
{
    /// <summary>
    /// Interaktionslogik für FloatEntryEditor.xaml
    /// </summary>
    public partial class FloatEntryEditor : UserControl
    {
        private readonly FloatEntry _entry;

        public FloatEntryEditor(FloatEntry entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = entry.varFloat.ToString();
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(ValueBox.Text, out float v))
                _entry.varFloat = v;
        }
    }
}
