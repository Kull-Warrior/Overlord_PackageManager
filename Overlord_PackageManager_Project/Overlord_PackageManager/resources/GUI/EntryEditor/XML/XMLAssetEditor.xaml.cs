using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
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

        public XMLAssetEditor(XMLEntry entry)
        {
            InitializeComponent();
            _entry = entry;
            BuildUI();
        }

        private void BuildUI()
        {
            List<Entry> entries = _entry.Table.Entries;

            StringEntry fileNameEntry = (StringEntry)entries[0];
            BlobEntry blobEntry = (BlobEntry)entries[2];

            StringEntryEditor fileNameEditor = new(fileNameEntry)
            {
                Label = "File Name"
            };

            HeaderPanel.Content = fileNameEditor;

            _xmlEditor = new XMLTextEditor();

            string xmlText = DecodeXML(blobEntry.Value);

            _xmlEditor.SetText(xmlText);
            _xmlEditor.XmlChanged += OnXmlChanged;

            EditorHost.Content = _xmlEditor;
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
            BlobEntry blobEntry = (BlobEntry)_entry.Table.Entries[2];

            byte[] newBytes = EncodeXML(newText);

            blobEntry.Value = newBytes;

            if (_entry.Table.Entries[1] is Int32Entry lengthEntry)
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

            _xmlEditor.SetText(xmlText);

            StringEntry fileNameEntry = (StringEntry)_entry.Table.Entries[0];
            fileNameEntry.Value = Path.GetFileName(dialog.FileName);

            OnXmlChanged(xmlText);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new();

            string fileName = ((StringEntry)_entry.Table.Entries[0]).Value;

            dialog.FileName = fileName;
            dialog.Filter = "XML Files (*.xml)|*.xml";

            if (dialog.ShowDialog() != true)
                return;

            BlobEntry blobEntry = (BlobEntry)_entry.Table.Entries[2];

            byte[] data = blobEntry.Value;

            if (data.Length > 0 && data[^1] == 0)
                data = data[..^1];

            File.WriteAllBytes(dialog.FileName, data);
        }
    }
}
