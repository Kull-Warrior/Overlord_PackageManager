using Microsoft.Win32;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.EntryTypes.Image.ReflectionCubeMap;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class ReflectionCubeMapEditor : UserControl
    {
        private readonly ReflectionCubeMapAsset _asset;

        private readonly List<CubemapMipLevel> _mips = new();
        private int _currentIndex = 0;

        public ReflectionCubeMapEditor(ReflectionCubeMapAsset asset)
        {
            InitializeComponent();
            _asset = asset;

            BuildMipLevels();
            SetupUI();
        }

        private void SetupUI()
        {
            var entries = _asset.Table.Entries;
            var stringEntries = entries.OfType<StringEntry>().ToList();

            if (stringEntries.Count >= 2)
            {
                var tagEditor = new StringEntryEditor(stringEntries[0])
                {
                    Label = "Tag"
                };

                var fileNameEditor = new StringEntryEditor(stringEntries[1])
                {
                    Label = "File Name"
                };

                // Insert at top of RootPanel
                RootPanel.Children.Insert(0, fileNameEditor);
                RootPanel.Children.Insert(0, tagEditor);
            }

            for (int i = 0; i < _mips.Count; i++)
                MipSelector.Items.Add($"Mip Level {i}");

            MipSelector.SelectionChanged += (s, e) =>
            {
                if (MipSelector.SelectedIndex >= 0)
                {
                    _currentIndex = MipSelector.SelectedIndex;
                    RenderCurrent();
                }
            };

            BtnImport.Click += Import_Click;
            BtnExport.Click += Export_Click;

            if (_mips.Count > 0)
                MipSelector.SelectedIndex = 0;
        }

        private void BuildMipLevels()
        {
            // Extract the raw DDSTextures
            var list = ((DDSTextureAssetDataContainer)_asset.Table.Entries[3])
                .Table.Entries.OfType<ListOfDDSTextures>().FirstOrDefault();

            if (list == null) return;

            List<DDSTextures> allTextures = list.Table.Entries.OfType<DDSTextures>().ToList();

            if (allTextures.Count == 0) return;

            // Determine mip count per face from first face
            uint width = ((Int32Entry)allTextures[0].Table.Entries[0]).varInt;
            uint height = ((Int32Entry)allTextures[0].Table.Entries[1]).varInt;
            DDSFormat format = (DDSFormat)((Int32Entry)allTextures[0].Table.Entries[2]).varInt;

            uint baseMipCount = DDSWriter.CalculateMipMapCount(width, height);

            if (allTextures.Count % baseMipCount != 0 || allTextures.Count % (baseMipCount * 6) != 0)
                throw new InvalidDataException("Cubemap texture count does not match expected 6-face x mip-count layout.");

            int mipCount = (int)baseMipCount;

            for (int mip = 0; mip < mipCount; mip++)
            {
                _mips.Add(new CubemapMipLevel
                {
                    Right = GetMipFace(allTextures, 0, mip, mipCount),
                    Left = GetMipFace(allTextures, 1, mip, mipCount),
                    Top = GetMipFace(allTextures, 2, mip, mipCount),
                    Bottom = GetMipFace(allTextures, 3, mip, mipCount),
                    Front = GetMipFace(allTextures, 4, mip, mipCount),
                    Back = GetMipFace(allTextures, 5, mip, mipCount),
                });
            }
        }

        private MipLevelData GetMipFace(List<DDSTextures> allTextures, int faceIndex, int mipIndex, int mipCount)
        {
            int index = faceIndex * mipCount + mipIndex;

            var tex = allTextures[index];

            var intEntries = tex.Table.Entries.OfType<Int32Entry>().TakeLast(3).ToList();
            uint width = intEntries[0].varInt;
            uint height = intEntries[1].varInt;
            DDSFormat format = (DDSFormat)intEntries[2].varInt;

            var blob = tex.Table.Entries.OfType<BlobEntry>().FirstOrDefault();
            if (blob == null) throw new InvalidDataException("Missing texture data");

            return new MipLevelData(width, height, format, blob.varBytes);
        }

        private void RenderCurrent()
        {
            if (_mips.Count == 0) return;

            var mip = _mips[_currentIndex];

            ImgRight.Source = CreateBitmap(mip.Right);
            ImgLeft.Source = CreateBitmap(mip.Left);
            ImgTop.Source = CreateBitmap(mip.Top);
            ImgBottom.Source = CreateBitmap(mip.Bottom);
            ImgFront.Source = CreateBitmap(mip.Front);
            ImgBack.Source = CreateBitmap(mip.Back);
        }

        private ImageSource CreateBitmap(MipLevelData mip)
        {
            byte[] rgba = DDSDecoder.Decode(mip.Width, mip.Height, mip.Format, mip.Data);

            WriteableBitmap bmp = new((int)mip.Width, (int)mip.Height, 96, 96, PixelFormats.Bgra32, null);
            bmp.WritePixels(new System.Windows.Int32Rect(0, 0, (int)mip.Width, (int)mip.Height), rgba, (int)mip.Width * 4, 0);

            return bmp;
        }

        private void Import_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "DDS Files (*.dds)|*.dds|All Files (*.*)|*.*";

            if (dialog.ShowDialog() != true)
                return;

            byte[] fileBytes = File.ReadAllBytes(dialog.FileName);

            _asset.ParseAndReplaceCubemapDDS(fileBytes);

            _mips.Clear();
            BuildMipLevels();
            RenderCurrent();
        }

        private void Export_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                _asset.WriteToFile(dialog.FolderName + "\\");
            }
        }
    }

    // Each mip level contains all six faces
    public class CubemapMipLevel
    {
        public MipLevelData Right;
        public MipLevelData Left;
        public MipLevelData Top;
        public MipLevelData Bottom;
        public MipLevelData Front;
        public MipLevelData Back;
    }
}