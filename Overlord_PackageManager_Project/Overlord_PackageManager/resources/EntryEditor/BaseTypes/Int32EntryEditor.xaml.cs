using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.EntryEditor
{
    /// <summary>
    /// Interaktionslogik für Int32EntryEditor.xaml
    /// </summary>
    public partial class Int32EntryEditor : UserControl
    {
        private readonly Int32Entry _entry;

        public Int32EntryEditor(Int32Entry entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = entry.varInt.ToString();
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            if (uint.TryParse(ValueBox.Text, out uint v))
                _entry.varInt = v;
        }
    }
}
