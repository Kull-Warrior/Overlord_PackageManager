using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.Generic;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.DDS
{
    public partial class DDSTextureAssetEditor : UserControl
    {
        private readonly DDSTextureAsset _asset;

        public DDSTextureAssetEditor(DDSTextureAsset asset)
        {
            InitializeComponent();
            _asset = asset;
            BuildUI();
        }

        private void BuildUI()
        {
            RootPanel.Children.Clear();

            List<Entry> entries = _asset.Table.Entries;

            List<CountedArrayEntry<char>> stringEntries = entries.OfType<CountedArrayEntry<char>>().ToList();
            DDSTextureAssetDataContainer? dataContainer = entries.OfType<DDSTextureAssetDataContainer>().FirstOrDefault();
            AssetListContainer? mipContainer = dataContainer.Table.Entries.OfType<AssetListContainer>().FirstOrDefault();

            if (stringEntries.Count >= 2)
            {
                StringEntryEditor tagEditor = new StringEntryEditor(stringEntries[0])
                {
                    Label = "Tag"
                };

                StringEntryEditor fileNameEditor = new StringEntryEditor(stringEntries[1])
                {
                    Label = "File Name"
                };

                RootPanel.Children.Add(tagEditor);
                RootPanel.Children.Add(fileNameEditor);
            }

            if (mipContainer != null)
            {
                AssetList? mipList = mipContainer.Table.Entries.OfType<AssetList>().FirstOrDefault();

                if (mipList != null)
                {
                    DDSMipChainEditor mipEditor = new DDSMipChainEditor(mipList);
                    RootPanel.Children.Add(mipEditor);
                }
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "DDS Files (*.dds)|*.dds|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(dialog.FileName);

            _asset.ReplaceFromDDS(fileBytes);

            // Update filename entry
            CountedArrayEntry<char> fileNameEntry = (CountedArrayEntry<char>)_asset.Table.Entries[1];
            fileNameEntry.Value = Path.GetFileName(dialog.FileName).ToCharArray();

            BuildUI();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();

            if (dialog.ShowDialog() != true)
                return;

            string fileName = new string(((CountedArrayEntry<char>)_asset.Table.Entries[1]).Value);

            if (!fileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                fileName += ".dds";

            string fullPath = Path.Combine(dialog.FolderName, fileName);

            using FileStream fs = File.Create(fullPath);
            _asset.WriteToDDS(fs);
        }
    }
}