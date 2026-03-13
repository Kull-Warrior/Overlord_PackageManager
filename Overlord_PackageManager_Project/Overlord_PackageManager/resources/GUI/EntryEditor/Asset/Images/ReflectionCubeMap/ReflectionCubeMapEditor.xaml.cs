using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.ReflectionCubeMap
{
    public partial class ReflectionCubeMapEditor : UserControl
    {
        private readonly ReflectionCubeMapAsset _asset;

        public ReflectionCubeMapEditor(ReflectionCubeMapAsset asset)
        {
            InitializeComponent();
            _asset = asset;

            BuildUI();
        }

        private void BuildUI()
        {
            RootPanel.Children.Clear();

            List<Entry> entries = _asset.Table.Entries;
            List<StringEntry> stringEntries = entries.OfType<StringEntry>().ToList();

            if (stringEntries.Count >= 2)
            {
                RootPanel.Children.Add(new StringEntryEditor(stringEntries[0])
                {
                    Label = "Tag"
                });

                RootPanel.Children.Add(new StringEntryEditor(stringEntries[1])
                {
                    Label = "File Name"
                });
            }

            // Load cubemap viewer (same pattern as DDS editor)
            RootPanel.Children.Add(new ReflectionCubeMapViewer(_asset));

            BtnImport.Click -= Import_Click;
            BtnImport.Click += Import_Click;

            BtnExport.Click -= Export_Click;
            BtnExport.Click += Export_Click;
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "DDS Files (*.dds)|*.dds|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
                return;

            byte[] fileBytes = File.ReadAllBytes(dialog.FileName);
            _asset.ReplaceFromDDS(fileBytes);

            // Update filename entry
            StringEntry fileNameEntry = (StringEntry)_asset.Table.Entries[1];
            fileNameEntry.Value = Path.GetFileName(dialog.FileName);

            BuildUI();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();

            if (dialog.ShowDialog() != true)
                return;

            string fileName = ((StringEntry)_asset.Table.Entries[1]).Value;

            if (!fileName.EndsWith(".dds", System.StringComparison.OrdinalIgnoreCase))
                fileName += ".dds";

            string fullPath = Path.Combine(dialog.FolderName, fileName);

            using FileStream fs = File.Create(fullPath);
            _asset.WriteToDDS(fs);
        }
    }
}