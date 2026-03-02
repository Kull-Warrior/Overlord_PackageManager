using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class DDSMipChainEditor : UserControl
    {
        private readonly List<MipLevelData> _mips = new();
        private int _currentIndex = 0;

        public DDSMipChainEditor(ListOfDDSTextures list)
        {
            InitializeComponent();

            MipSelector.SelectionChanged += MipSelector_SelectionChanged;
            PreviewImage.MouseLeftButtonDown += PreviewImage_MouseLeftButtonDown;

            List<DDSTextures> ddsEntries = list.Table.Entries.OfType<DDSTextures>().ToList();

            BuildFromDDSList(ddsEntries);

            if (_mips.Count > 0)
                MipSelector.SelectedIndex = 0;
        }

        public DDSMipChainEditor(List<DDSTextures> ddsEntries)
        {
            InitializeComponent();

            MipSelector.SelectionChanged += MipSelector_SelectionChanged;
            PreviewImage.MouseLeftButtonDown += PreviewImage_MouseLeftButtonDown;

            BuildFromDDSList(ddsEntries);

            if (_mips.Count > 0)
                MipSelector.SelectedIndex = 0;
        }

        private void BuildFromDDSList(List<DDSTextures> ddsEntries)
        {
            int level = 0;

            foreach (DDSTextures dds in ddsEntries)
            {
                List<Int32Entry> intEntries = dds.Table.Entries.OfType<Int32Entry>().ToList();

                if (intEntries.Count < 3)
                    continue;

                List<Int32Entry> lastThree = intEntries.TakeLast(3).ToList();

                uint width = lastThree[0].varInt;
                uint height = lastThree[1].varInt;
                DDSFormat format = (DDSFormat)lastThree[2].varInt;

                BlobEntry? blob = dds.Table.Entries.OfType<BlobEntry>().FirstOrDefault();

                if (blob == null)
                {
                    continue;
                }

                _mips.Add(new MipLevelData(width, height, format, blob.varBytes));

                MipSelector.Items.Add($"Level {level}  ({width}x{height})");

                level++;
            }
        }

        private void MipSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MipSelector.SelectedIndex < 0)
            {
                return;
            }

            _currentIndex = MipSelector.SelectedIndex;
            RenderCurrent();
        }

        private void RenderCurrent()
        {
            if (_mips.Count == 0)
            {
                return;
            }

            MipLevelData mip = _mips[_currentIndex];

            byte[] rgba = DDSDecoder.Decode(mip.Width, mip.Height, mip.Format, mip.Data);

            WriteableBitmap bmp = new WriteableBitmap((int)mip.Width, (int)mip.Height, 96, 96, PixelFormats.Bgra32, null);

            bmp.WritePixels(new Int32Rect(0, 0, (int)mip.Width, (int)mip.Height), rgba, (int)mip.Width * 4, 0);

            PreviewImage.Source = bmp;

            WidthText.Text = $"Width: {mip.Width}";
            HeightText.Text = $"Height: {mip.Height}";
            FormatText.Text = $"{mip.Format}";
        }

        private void PreviewImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && _mips.Count > 0)
            {
                MipLevelData mip = _mips[_currentIndex];
                DDSZoomWindow window = new DDSZoomWindow(mip);
                window.Show();
            }
        }
    }
}