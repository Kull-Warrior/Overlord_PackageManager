using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.XML;
using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.XML
{
    public partial class XMLAssetEditor : UserControl
    {
        private readonly XMLEntry _entry;
        private XMLTextEditor _xmlEditor;
        private bool HasValidEntries => _entry?.Table?.Entries != null && _entry.Table.Entries.Count >= 3;

        public XMLAssetEditor(XMLEntry entry)
        {
            InitializeComponent();
            _entry = entry;
            BuildUI();
            UpdateButtonStates();
        }

        private void BuildUI()
        {
            if (!HasValidEntries)
            {
                EditorHost.Content = null;
                HeaderPanel.Content = null;
                return;
            }

            List<Entry> entries = _entry.Table.Entries;

            CharCountedArrayEntry fileNameEntry = (CharCountedArrayEntry)entries[0];
            ByteArrayEntry data = (ByteArrayEntry)entries[2];

            StringEntryEditor fileNameEditor = new(fileNameEntry)
            {
                Label = "File Name"
            };

            HeaderPanel.Content = fileNameEditor;

            _xmlEditor = new XMLTextEditor();

            string xmlText = DecodeXML(data.Value);

            _xmlEditor.SetText(xmlText);
            _xmlEditor.XmlChanged += OnXmlChanged;

            EditorHost.Content = _xmlEditor;
        }

        private void UpdateButtonStates()
        {
            ExportButton.IsEnabled = HasValidEntries;

            // Import should always remain enabled
            ImportButton.IsEnabled = true;
        }

        private void CreateEntries(string fileName, string xmlText)
        {
            byte[] data = EncodeXML(xmlText);

            _entry.Table.Entries.Clear();
            
            _entry.Table.Entries.Add(new CharCountedArrayEntry(10, 0) { Value = fileName.ToCharArray() });
            _entry.Table.Entries.Add(new UInt32Entry(11, (uint)_entry.Table.Entries.Sum(e => e.PayloadLength)) { Value = (uint)data.Length });
            _entry.Table.Entries.Add(new ByteArrayEntry(12, (uint)_entry.Table.Entries.Sum(e => e.PayloadLength)) { Value = data });
        }

        private string DecodeXML(byte[] data)
        {
            if (data.Length > 0 && data[^1] == 0)
                data = data[..^1];

            return System.Text.Encoding.UTF8.GetString(data);
        }

        private byte[] EncodeXML(string text)
        {
            byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(text);

            byte[] result = new byte[xmlBytes.Length + 1];
            Buffer.BlockCopy(xmlBytes, 0, result, 0, xmlBytes.Length);

            result[^1] = 0; // null terminator

            return result;
        }

        private void OnXmlChanged(string newText)
        {
            if (!HasValidEntries)
            {
                return;
            }

            ByteArrayEntry data = (ByteArrayEntry)_entry.Table.Entries[2];

            byte[] newBytes = EncodeXML(newText);

            data.Value = newBytes;

            if (_entry.Table.Entries[1] is UInt32Entry lengthEntry)
            {
                lengthEntry.Value = (uint)newBytes.Length;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != true)
                return;

            string xmlText = File.ReadAllText(dialog.FileName);

            string fileName = Path.GetFileName(dialog.FileName);

            // Create entries if missing
            if (!HasValidEntries)
            {
                CreateEntries(fileName, xmlText);

                BuildUI();
            }
            else
            {
                ((CharCountedArrayEntry)_entry.Table.Entries[0]).Value = fileName.ToCharArray();
            }

            _xmlEditor.SetText(xmlText);

            OnXmlChanged(xmlText);

            UpdateButtonStates();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (!HasValidEntries)
            {
                MessageBox.Show(
                    "Nothing to export.",
                    "Export XML",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            SaveFileDialog dialog = new();

            char[] fileNameChars = ((CharCountedArrayEntry)_entry.Table.Entries[0]).Value;
            string fileName = new string(fileNameChars);

            dialog.FileName = fileName;
            dialog.Filter = "XML Files (*.xml)|*.xml";

            if (dialog.ShowDialog() != true)
                return;

            ByteArrayEntry rawData = (ByteArrayEntry)_entry.Table.Entries[2];

            byte[] xmlData = rawData.Value;

            if (xmlData.Length > 0 && xmlData[^1] == 0)
                xmlData = xmlData[..^1];

            File.WriteAllBytes(dialog.FileName, xmlData);
        }
    }
}
