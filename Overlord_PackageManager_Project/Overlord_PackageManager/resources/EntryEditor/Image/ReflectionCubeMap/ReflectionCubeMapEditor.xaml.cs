using Microsoft.Win32;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
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
            //RootPanel.Children.Clear();

            List<Generic.Entry> entries = _asset.Table.Entries;
            List<StringEntry> stringEntries = entries.OfType<StringEntry>().ToList();

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

                RootPanel.Children.Insert(0, fileNameEditor);
                RootPanel.Children.Insert(0, tagEditor);
            }

            MipSelector.Items.Clear();

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
            _mips.Clear();

            ListOfDDSTextures? list = ((DDSTextureAssetDataContainer)_asset.Table.Entries[3]).Table.Entries.OfType<ListOfDDSTextures>().FirstOrDefault();

            if (list == null) return;

            List<DDSTextures> textures = list.Table.Entries.OfType<DDSTextures>().ToList();
            if (textures.Count == 0)
            {
                return;
            }

            int mipCount = textures.Count / 6;

            for (int mip = 0; mip < mipCount; mip++)
            {
                _mips.Add(new CubemapMipLevel
                {
                    Right = GetFace(textures, 0, mip, mipCount),
                    Left = GetFace(textures, 1, mip, mipCount),
                    Top = GetFace(textures, 2, mip, mipCount),
                    Bottom = GetFace(textures, 3, mip, mipCount),
                    Front = GetFace(textures, 4, mip, mipCount),
                    Back = GetFace(textures, 5, mip, mipCount),
                });
            }
        }

        private MipLevelData GetFace(List<DDSTextures> textures, int faceIndex, int mipIndex, int mipCount)
        {
            int index = faceIndex * mipCount + mipIndex;
            DDSTextures tex = textures[index];

            List<Int32Entry> intEntries = tex.Table.Entries.OfType<Int32Entry>().Take(3).ToList();

            uint width = intEntries[0].varInt;
            uint height = intEntries[1].varInt;
            DDSFormat format = (DDSFormat)intEntries[2].varInt;

            BlobEntry blob = tex.Table.Entries.OfType<BlobEntry>().First();

            return new MipLevelData(width, height, format, blob.varBytes);
        }

        private void RenderCurrent()
        {
            if (_mips.Count == 0) return;

            CubemapMipLevel mip = _mips[_currentIndex];

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

            WriteableBitmap bmp = new(
                (int)mip.Width,
                (int)mip.Height,
                96, 96,
                PixelFormats.Bgra32,
                null);

            bmp.WritePixels(
                new Int32Rect(0, 0, (int)mip.Width, (int)mip.Height),
                rgba,
                (int)mip.Width * 4,
                0);

            return bmp;
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

            StringEntry fileNameEntry = (StringEntry)_asset.Table.Entries[1];
            fileNameEntry.varString = Path.GetFileName(dialog.FileName);

            BuildMipLevels();
            RenderCurrent();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() != true)
            {
                return;
            }
            
            string fileName = ((StringEntry)_asset.Table.Entries[1]).varString;
            
            if (!fileName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".dds";
            }

            string fullPath = Path.Combine(dialog.FolderName, fileName);

            using FileStream fs = File.Create(fullPath);
            {
                _asset.WriteToDDS(fs);
            }
        }
    }

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