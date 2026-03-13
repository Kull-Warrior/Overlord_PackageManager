using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    /// <summary>
    /// Interaktionslogik für Int64EntryEditor.xaml
    /// </summary>
    public partial class Int64EntryEditor : UserControl
    {
        private readonly Int64Entry _entry;

        public Int64EntryEditor(Int64Entry entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = entry.Value.ToString();
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            if (ulong.TryParse(ValueBox.Text, out ulong v))
                _entry.Value = v;
        }
    }
}