using Microsoft.Win32;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.EntryEditor
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
            // Assuming asset exposes a ReferenceTable like others
            List<Generic.Entry> entries = _asset.Table.Entries;

            List<StringEntry> stringEntries = entries.OfType<StringEntry>().ToList();

            DDSTextureAssetDataContainer? mipContainer = entries.OfType<DDSTextureAssetDataContainer>().FirstOrDefault();

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
                ListOfDDSTextures? mipList = mipContainer.Table.Entries.OfType<ListOfDDSTextures>().FirstOrDefault();

                if (mipList != null)
                {
                    DDSMipChainEditor mipEditor = new DDSMipChainEditor(mipList);
                    RootPanel.Children.Add(mipEditor);
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                _asset.WriteToFile(dialog.FolderName + "\\");
            }
        }

        private uint CalculateMipByteSize(uint width, uint height, DDSFormat format)
        {
            if (format == DDSFormat.UncompressedRGB)
                return width * height * 3;

            if (format == DDSFormat.UncompressedRGBA)
                return width * height * 4;

            uint blockSize = format == DDSFormat.DXT1 ? 8u : 16u;

            uint blocksWide = (width + 3) / 4;
            uint blocksHigh = (height + 3) / 4;

            return blocksWide * blocksHigh * blockSize;
        }

        private void ReplaceMipChain(BinaryReader br, uint width, uint height, uint mipCount, DDSFormat format)
        {
            DDSTextureAssetDataContainer container = (DDSTextureAssetDataContainer)_asset.Table.Entries[3];

            ListOfDDSTextures list = (ListOfDDSTextures)container.Table.Entries[0];

            list.Table.Entries.Clear();

            uint currentWidth = width;
            uint currentHeight = height;

            for (int i = 0; i < mipCount; i++)
            {
                uint byteLength = CalculateMipByteSize(currentWidth, currentHeight, format);

                byte[] mipData = br.ReadBytes((int)byteLength);

                DDSTextures ddsEntry = new DDSTextures(currentWidth, currentHeight, format, mipData);

                list.Table.Entries.Add(ddsEntry);

                currentWidth = Math.Max(1, currentWidth / 2);
                currentHeight = Math.Max(1, currentHeight / 2);
            }
        }

        private void ParseAndReplaceDDS(byte[] fileBytes, string filePath)
        {
            using MemoryStream ms = new MemoryStream(fileBytes);
            using BinaryReader br = new BinaryReader(ms);
            {
                // Verify header
                string magic = new string(br.ReadChars(4));
                if (magic != "DDS ")
                    throw new InvalidDataException("Not a valid DDS file");

                br.BaseStream.Position = 12;
                uint height = br.ReadUInt32();
                uint width = br.ReadUInt32();
                br.BaseStream.Position = 28;
                uint mipMapCount = br.ReadUInt32();

                br.BaseStream.Position = 80;
                uint pfFlags = br.ReadUInt32();
                string fourCC = new string(br.ReadChars(4));

                DDSFormat format;

                if ((pfFlags & 0x4) != 0) // compressed
                {
                    format = fourCC switch
                    {
                        "DXT1" => DDSFormat.DXT1,
                        "DXT3" => DDSFormat.DXT3,
                        "DXT5" => DDSFormat.DXT5,
                        _ => throw new NotSupportedException("Unsupported DDS format")
                    };
                }
                else
                {
                    br.BaseStream.Position = 88;
                    uint bitCount = br.ReadUInt32();

                    format = bitCount == 32
                        ? DDSFormat.UncompressedRGBA
                        : DDSFormat.UncompressedRGB;
                }

                br.BaseStream.Position = 128;

                // Update filename
                StringEntry fileNameEntry = (StringEntry)_asset.Table.Entries[1];
                fileNameEntry.varString = Path.GetFileName(filePath);

                ReplaceMipChain(br, width, height, mipMapCount, format);

                // Rebuild UI
                RootPanel.Children.Clear();
                BuildUI();
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "RPK Files (*.rpk)|*.rpk|" +
                "PRP Files (*.prp)|*.prp|" +
                "PSP Files (*.psp)|*.psp|" +
                "PVP Files (*.pvp)|*.pvp|" +
                "OMP Files (*.omp)|*.omp|" +
                "All files (*.*)| *.*";
            openFileDialog.FilterIndex = 6;
            if (openFileDialog.ShowDialog() == true)
            {
                
            }

            OpenFileDialog dialog = new OpenFileDialog();
            openFileDialog.Filter = "DDS Files (*.dds)|*.dds|" +
                "All files (*.*)| *.*";

            if (dialog.ShowDialog() != true)
                return;

            byte[] fileBytes = File.ReadAllBytes(dialog.FileName);

            ParseAndReplaceDDS(fileBytes, dialog.FileName);
        }
    }
}