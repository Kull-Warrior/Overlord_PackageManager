using Overlord_PackageManager.resources.EntryTypes;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class ReflectionCubeMapViewer : UserControl
    {
        private readonly ReflectionCubeMapAsset _asset;
        private readonly List<CubemapMipLevel> _mips = new();
        private int _currentIndex = 0;

        public ReflectionCubeMapViewer(ReflectionCubeMapAsset asset)
        {
            InitializeComponent();
            _asset = asset;

            MipSelector.SelectionChanged += MipSelector_SelectionChanged;

            BuildMipLevels();
            BuildPreviewGrid();

            if (_mips.Count > 0)
                MipSelector.SelectedIndex = 0;
        }

        private void BuildMipLevels()
        {
            _mips.Clear();

            AssetList? list = ((DDSTextureAssetDataContainer)_asset.Table.Entries[3])
                .Table.Entries
                .OfType<AssetList>()
                .FirstOrDefault();

            if (list == null)
                return;

            List<DDSTextures> textures = list.Table.Entries.OfType<DDSTextures>().ToList();

            if (textures.Count == 0)
                return;

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

                MipSelector.Items.Add($"Mip Level {mip}");
            }
        }

        private MipLevelData GetFace(List<DDSTextures> textures, int faceIndex, int mipIndex, int mipCount)
        {
            int index = faceIndex * mipCount + mipIndex;
            DDSTextures tex = textures[index];

            List<Int32Entry> intEntries = tex.Table.Entries.OfType<Int32Entry>().Take(3).ToList();

            uint width = intEntries[0].Value;
            uint height = intEntries[1].Value;
            DDSFormat format = (DDSFormat)intEntries[2].Value;

            BlobEntry blob = tex.Table.Entries.OfType<BlobEntry>().First();

            return new MipLevelData(width, height, format, blob.Data);
        }

        private void BuildPreviewGrid()
        {
            PreviewGrid.RowDefinitions.Clear();
            PreviewGrid.ColumnDefinitions.Clear();
            PreviewGrid.Children.Clear();

            // Rows
            for (int i = 0; i < 3; i++)
                PreviewGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Columns:
            // Spacer | Left | Front | Right | Back | Spacer
            PreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Left spacer
            PreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            PreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            PreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            PreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            PreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Right spacer

            AddImage("Top", 0, 2);
            AddImage("Left", 1, 1);
            AddImage("Front", 1, 2);
            AddImage("Right", 1, 3);
            AddImage("Back", 1, 4);
            AddImage("Bottom", 2, 2);
        }

        private void AddImage(string name, int row, int col)
        {
            Image img = new()
            {
                Name = name,
                MaxWidth = 128,
                MaxHeight = 128
            };

            PreviewGrid.Children.Add(img);
            Grid.SetRow(img, row);
            Grid.SetColumn(img, col);
        }

        private void MipSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MipSelector.SelectedIndex < 0)
                return;

            _currentIndex = MipSelector.SelectedIndex;
            RenderCurrent();
        }

        private void RenderCurrent()
        {
            if (_mips.Count == 0)
                return;

            CubemapMipLevel mip = _mips[_currentIndex];

            foreach (Image img in PreviewGrid.Children.OfType<Image>())
            {
                img.Source = img.Name switch
                {
                    "Top" => CreateBitmap(mip.Top),
                    "Bottom" => CreateBitmap(mip.Bottom),
                    "Front" => CreateBitmap(mip.Front),
                    "Back" => CreateBitmap(mip.Back),
                    "Left" => CreateBitmap(mip.Left),
                    "Right" => CreateBitmap(mip.Right),
                    _ => null
                };
            }
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