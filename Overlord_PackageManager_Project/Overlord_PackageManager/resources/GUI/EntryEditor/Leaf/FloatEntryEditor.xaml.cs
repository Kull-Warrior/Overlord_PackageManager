using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    /// <summary>
    /// Interaktionslogik für FloatEntryEditor.xaml
    /// </summary>
    public partial class FloatEntryEditor : UserControl
    {
        private readonly ScalarEntry<float> _entry;

        public FloatEntryEditor(ScalarEntry<float> entry)
        {
            InitializeComponent();

            _entry = entry;
            ValueBox.Text = entry.Value.ToString();
            ValueBox.TextChanged += ValueChanged;
        }

        private void ValueChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(ValueBox.Text, out float v))
                _entry.Value = v;
        }
    }
}