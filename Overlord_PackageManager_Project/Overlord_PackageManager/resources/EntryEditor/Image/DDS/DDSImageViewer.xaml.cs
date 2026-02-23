using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class DDSImageViewer : UserControl
    {
        public DDSImageViewer(uint width, uint height, DDSFormat format, byte[] rawData)
        {
            InitializeComponent();

            byte[] rgbaData = DDSDecoder.Decode(width, height, format, rawData);

            var bitmap = new WriteableBitmap(
                (int)width,
                (int)height,
                96,
                96,
                PixelFormats.Bgra32,
                null);

            bitmap.WritePixels(
                new System.Windows.Int32Rect(0, 0, (int)width, (int)height),
                rgbaData,
                (int)width * 4,
                0);

            ImageControl.Source = bitmap;
        }
    }
}