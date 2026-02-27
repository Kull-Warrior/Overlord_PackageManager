using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class DDSZoomWindow : Window
    {
        private double _scale = 1.0;
        private Point _start;
        private Point _origin;

        public DDSZoomWindow(MipLevelData mip)
        {
            InitializeComponent();

            byte[] rgba = DDSDecoder.Decode(
                mip.Width,
                mip.Height,
                mip.Format,
                mip.Data);

            var bmp = new WriteableBitmap(
                (int)mip.Width,
                (int)mip.Height,
                96,
                96,
                PixelFormats.Bgra32,
                null);

            bmp.WritePixels(
                new Int32Rect(0, 0, (int)mip.Width, (int)mip.Height),
                rgba,
                (int)mip.Width * 4,
                0);

            ZoomImage.Source = bmp;

            ZoomImage.MouseWheel += ZoomImage_MouseWheel;
            ZoomImage.MouseLeftButtonDown += ZoomImage_MouseLeftButtonDown;
            ZoomImage.MouseLeftButtonUp += (_, __) => ZoomImage.ReleaseMouseCapture();
            ZoomImage.MouseMove += ZoomImage_MouseMove;
        }

        private void ZoomImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _scale *= e.Delta > 0 ? 1.1 : 0.9;

            if (_scale < 0.1)
                _scale = 0.1;

            ScaleTransform.ScaleX = _scale;
            ScaleTransform.ScaleY = _scale;
        }

        private void ZoomImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ZoomImage.CaptureMouse();
            _start = e.GetPosition(Scroll);
            _origin = new Point(Scroll.HorizontalOffset, Scroll.VerticalOffset);
        }

        private void ZoomImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!ZoomImage.IsMouseCaptured)
                return;

            Point current = e.GetPosition(Scroll);

            Scroll.ScrollToHorizontalOffset(_origin.X - (current.X - _start.X));
            Scroll.ScrollToVerticalOffset(_origin.Y - (current.Y - _start.Y));
        }
    }
}