using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    /// <summary>
    /// Interaktionslogik für UInt32EntryEditor.xaml
    /// </summary>
    public partial class UInt32EntryEditor : UserControl
    {
        private readonly UInt32Entry _entry;

        public UInt32EntryEditor(UInt32Entry entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = entry.Value.ToString();
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            if (uint.TryParse(ValueBox.Text, out uint v))
                _entry.Value = v;
        }
    }
}