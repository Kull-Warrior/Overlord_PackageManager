using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// Interaction logic for FloatArrayEntryEditor.xaml
    /// </summary>
    public partial class FloatArrayEntryEditor : UserControl
    {
        private readonly RawArrayEntry<float> _entry;

        public FloatArrayEntryEditor(RawArrayEntry<float> entry)
        {
            InitializeComponent();

            _entry = entry;

            BuildUI();
        }

        private void BuildUI()
        {
            FloatGrid.Children.Clear();

            for (int i = 0; i < _entry.Value.Length; i++)
            {
                int index = i;

                TextBox box = new TextBox();
                box.Margin = new System.Windows.Thickness(4);
                box.Text = _entry.Value[i].ToString();

                box.TextChanged += (s, e) =>
                {
                    if (float.TryParse(box.Text, out float value))
                    {
                        _entry.Value[index] = value;
                    }
                };

                FloatGrid.Children.Add(box);
            }
        }
    }
}