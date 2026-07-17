using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    /// <summary>
    /// Interaktionslogik für UInt64EntryEditor.xaml
    /// </summary>
    public partial class UInt64EntryEditor : UserControl
    {
        private readonly UInt64Entry _entry;

        public UInt64EntryEditor(UInt64Entry entry)
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