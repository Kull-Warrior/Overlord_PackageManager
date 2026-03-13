using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class StringListEntryEditor : UserControl
    {
        private readonly StringListEntry _entry;
        private bool _isInternalUpdate;

        public StringListEntryEditor(StringListEntry entry)
        {
            InitializeComponent();
            _entry = entry;

            LoadText();
        }

        private void LoadText()
        {
            _isInternalUpdate = true;

            LinesBox.Text = string.Join(Environment.NewLine,
                _entry.Value.Select(l => l.Text));

            _isInternalUpdate = false;
        }

        private void LinesBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInternalUpdate)
                return;

            RebuildEntry();
        }

        private void RebuildEntry()
        {
            string[] lines = LinesBox.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            List<StringLine> newLines = new();

            foreach (string line in lines)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(line);
                newLines.Add(new StringLine((uint)bytes.Length, line));
            }

            _entry.Value = newLines;
            _entry.NumberOfLines = (uint)newLines.Count;
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string text = File.ReadAllText(dialog.FileName);

            LinesBox.Text = text;

            RebuildEntry();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new();
            dialog.Filter = "Text Files (*.txt)|*.txt";

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            File.WriteAllText(dialog.FileName, LinesBox.Text);
        }
    }
}
