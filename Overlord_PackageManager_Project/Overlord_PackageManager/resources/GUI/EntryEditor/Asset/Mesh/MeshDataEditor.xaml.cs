using HelixToolkit.Wpf;
using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Mesh
{
    public partial class MeshDataEditor : UserControl
    {
        private readonly MeshData _meshData;

        private MeshGeometry3D? _mesh;
        private BitmapSource? _uvPreview;

        public MeshDataEditor(MeshData meshData)
        {
            InitializeComponent();
            _meshData = meshData;

            Loaded += (_, __) => Build();
        }

        private void Build()
        {
            RootPanel.Children.Clear();

            VertexBuffer? vertexBuffer = _meshData.Table.Entries.OfType<VertexBuffer>().FirstOrDefault();
            IndiceData? indiceData = _meshData.Table.Entries.OfType<IndiceData>().FirstOrDefault();

            RootPanel.Children.Add(new TextBlock { Text = $"VertexBuffer: {vertexBuffer != null}" });
            RootPanel.Children.Add(new TextBlock { Text = $"IndiceData: {indiceData != null}" });

            Rebuild();
        }

        private void Rebuild()
        {
            _mesh = BuildMesh();
            BuildViewport();
            BuildUvPreview();
        }

        private MeshGeometry3D? BuildMesh()
        {
            VertexBuffer? vertexBuffer = _meshData.Table.Entries.OfType<VertexBuffer>().FirstOrDefault();
            IndiceData? indiceData = _meshData.Table.Entries.OfType<IndiceData>().FirstOrDefault();

            if (vertexBuffer == null || indiceData == null)
            {
                return null;
            }

            BlobEntry? blob = vertexBuffer.Table.Entries.OfType<BlobEntry>().FirstOrDefault();
            Int32Entry? vertexCountEntry = vertexBuffer.Table.Entries.OfType<Int32Entry>().FirstOrDefault(e => e.Id == 21);

            VertexBufferInfo? info = vertexBuffer.Table.Entries.OfType<VertexBufferInfo>().FirstOrDefault();
            Int32Entry? strideEntry = info?.Table.Entries.OfType<Int32Entry>().FirstOrDefault(e => e.Id == 21);
            VertexDeclarationEntry? decl = info?.Table.Entries.OfType<VertexDeclarationEntry>().FirstOrDefault();

            UInt16ArrayEntry? indicesEntry = indiceData.Table.Entries.OfType<UInt16ArrayEntry>().FirstOrDefault();

            if (blob?.Value == null || vertexCountEntry == null || strideEntry == null || decl == null || indicesEntry == null)
            {
                return null;
            }

            int stride = (int)strideEntry.Value;
            int vertexCount = (int)vertexCountEntry.Value;

            MeshGeometry3D mesh = new MeshGeometry3D();

            byte[] data = blob.Value;

            for (int i = 0; i < vertexCount; i++)
            {
                int baseOffset = i * stride;
                if (baseOffset + stride > data.Length)
                {
                    break;
                }

                int offset = baseOffset;

                Point3D? pos = null;
                Point? uv = null;

                foreach (VertexAttribute attr in decl.Value)
                {
                    switch (attr.Semantic)
                    {
                        case VertexAttributeSemantic.Position:
                            pos = ReadFloat3(data, ref offset);
                            break;

                        case VertexAttributeSemantic.TexCoord:
                            uv = ReadFloat2(data, ref offset);
                            break;

                        default:
                            offset += attr.ByteSize;
                            break;
                    }
                }

                if (pos.HasValue)
                {
                    mesh.Positions.Add(pos.Value);
                    mesh.TextureCoordinates.Add(uv ?? new Point());
                }
            }

            foreach (ushort idx in indicesEntry.Value)
            {
                if (idx < mesh.Positions.Count)
                {
                    mesh.TriangleIndices.Add(idx);
                }
            }

            return mesh;
        }

        private void BuildViewport()
        {
            Viewport.Children.Clear();
            Viewport.Children.Add(new DefaultLights());

            if (_mesh == null || _mesh.Positions.Count == 0)
            {
                return;
            }

            Material material = MaterialHelper.CreateMaterial(Brushes.LightGray);

            GeometryModel3D model = new GeometryModel3D
            {
                Geometry = _mesh,
                Material = material,
                BackMaterial = material
            };

            Viewport.Children.Add(new ModelVisual3D { Content = model });
            Viewport.ZoomExtents();
        }

        private void BuildUvPreview()
        {
            if (_mesh == null || _mesh.TextureCoordinates.Count == 0 || _mesh.TriangleIndices.Count == 0)
            {
                ShowPlaceholder("No UV data");
                return;
            }

            const int size = 1024;

            double minU = double.MaxValue, minV = double.MaxValue;
            double maxU = double.MinValue, maxV = double.MinValue;

            foreach (Point uv in _mesh.TextureCoordinates)
            {
                minU = Math.Min(minU, uv.X);
                minV = Math.Min(minV, uv.Y);
                maxU = Math.Max(maxU, uv.X);
                maxV = Math.Max(maxV, uv.Y);
            }

            double rangeU = maxU - minU;
            double rangeV = maxV - minV;

            if (rangeU < 1e-6)
            {
                rangeU = 1.0;
            }
            if (rangeV < 1e-6)
            {
                rangeV = 1.0;
            }

            double margin = 0.1;
            minU -= rangeU * margin;
            maxU += rangeU * margin;
            minV -= rangeV * margin;
            maxV += rangeV * margin;

            Point ToPixel(Point uv)
            {
                double nx = (uv.X - minU) / (maxU - minU);
                double ny = 1.0 - (uv.Y - minV) / (maxV - minV); // flip V for image
                return new Point(nx * (size - 1), ny * (size - 1));
            }

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            try
            {
                DrawCheckerboard(drawingContext, size);

                // Triangles filled with bright yellow (semi‑transparent) and solid white outline
                SolidColorBrush fillBrush = new SolidColorBrush(Color.FromArgb(120, 255, 255, 0)); // bright yellow
                Pen outlinePen = new Pen(Brushes.White, 3.0);

                for (int i = 0; i < _mesh.TriangleIndices.Count; i += 3)
                {
                    int i0 = _mesh.TriangleIndices[i];
                    int i1 = _mesh.TriangleIndices[i + 1];
                    int i2 = _mesh.TriangleIndices[i + 2];

                    if (i0 >= _mesh.TextureCoordinates.Count || i1 >= _mesh.TextureCoordinates.Count || i2 >= _mesh.TextureCoordinates.Count)
                    {
                        continue;
                    }

                    Point p0 = ToPixel(_mesh.TextureCoordinates[i0]);
                    Point p1 = ToPixel(_mesh.TextureCoordinates[i1]);
                    Point p2 = ToPixel(_mesh.TextureCoordinates[i2]);

                    // Skip degenerate triangles
                    if (Math.Abs(p0.X - p1.X) < 0.5 && Math.Abs(p0.Y - p1.Y) < 0.5 && Math.Abs(p0.X - p2.X) < 0.5 && Math.Abs(p0.Y - p2.Y) < 0.5)
                    {
                        continue;
                    }

                    StreamGeometry geometry = new StreamGeometry();
                    using (StreamGeometryContext geometryContext = geometry.Open())
                    {
                        geometryContext.BeginFigure(p0, true, true);
                        geometryContext.LineTo(p1, true, false);
                        geometryContext.LineTo(p2, true, false);
                    }

                    drawingContext.DrawGeometry(fillBrush, outlinePen, geometry);
                }
            }
            finally
            {
                drawingContext.Close();
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(size, size, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            bitmap.Freeze();

            _uvPreview = bitmap;
            UvImage.Source = _uvPreview;
        }

        private void ShowPlaceholder(string message)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Black, null, new Rect(0, 0, 512, 256));
                FormattedText text = new FormattedText(message, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 14, Brushes.White, 96);
                drawingContext.DrawText(text, new Point(20, 100));
            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(512, 256, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            bitmap.Freeze();
            _uvPreview = bitmap;
            UvImage.Source = _uvPreview;
        }

        // Draws an opaque 8x8 checkerboard using fully opaque colours
        private static void DrawCheckerboard(DrawingContext dc, int size)
        {
            int tiles = 8;
            double tileSize = (double)size / tiles;
            SolidColorBrush darkBrush = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            SolidColorBrush lightBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));

            for (int row = 0; row < tiles; row++)
            {
                for (int col = 0; col < tiles; col++)
                {
                    Rect rect = new Rect(col * tileSize, row * tileSize, tileSize, tileSize);
                    dc.DrawRectangle(((row + col) % 2 == 0) ? darkBrush : lightBrush, null, rect);
                }
            }
        }

        private static Point3D ReadFloat3(byte[] data, ref int offset)
        {
            float x = BitConverter.ToSingle(data, offset);
            offset += 4;
            float y = BitConverter.ToSingle(data, offset);
            offset += 4;
            float z = BitConverter.ToSingle(data, offset);
            offset += 4;

            return new Point3D(x, y, z);
        }

        private static Point ReadFloat2(byte[] data, ref int offset)
        {
            float x = BitConverter.ToSingle(data, offset);
            offset += 4;
            float y = BitConverter.ToSingle(data, offset);
            offset += 4;

            return new Point(x, 1.0 - y); // flip V from bottom‑left origin to image top‑left origin
        }

        private void ExportUv_Click(object sender, RoutedEventArgs e)
        {
            if (_uvPreview == null)
            {
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PNG (*.png)|*.png",
                FileName = "uv.png"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_uvPreview));

            using FileStream fileStream = File.Create(dialog.FileName);
            encoder.Save(fileStream);
        }

        private void Rebuild_Click(object sender, RoutedEventArgs e)
        {
            Rebuild();
        }
    }
}