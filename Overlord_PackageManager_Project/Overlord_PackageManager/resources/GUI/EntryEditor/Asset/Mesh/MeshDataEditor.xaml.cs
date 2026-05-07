using HelixToolkit.Wpf;
using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using System.IO;
using System.Text;
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

            // Show detailed info on the Info tab
            BuildInfoPanel(vertexBuffer, indiceData);

            Rebuild();
        }

        private void BuildInfoPanel(VertexBuffer? vertexBuffer, IndiceData? indiceData)
        {
            RootPanel.Children.Clear();

            if (vertexBuffer == null)
                RootPanel.Children.Add(new TextBlock { Text = "VertexBuffer: NOT FOUND", Foreground = Brushes.Red });
            else
                RootPanel.Children.Add(new TextBlock { Text = "VertexBuffer: FOUND", Foreground = Brushes.Green });

            if (indiceData == null)
                RootPanel.Children.Add(new TextBlock { Text = "IndiceData: NOT FOUND", Foreground = Brushes.Red });
            else
                RootPanel.Children.Add(new TextBlock { Text = "IndiceData: FOUND", Foreground = Brushes.Green });

            // If we have a built mesh, show actual stats
            if (_mesh != null)
            {
                RootPanel.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10) });
                RootPanel.Children.Add(new TextBlock { Text = $"Vertex count: {_mesh.Positions.Count}", FontWeight = FontWeights.Bold });
                RootPanel.Children.Add(new TextBlock { Text = $"Triangle count: {_mesh.TriangleIndices.Count / 3}" });
                RootPanel.Children.Add(new TextBlock { Text = $"Index count: {_mesh.TriangleIndices.Count}" });
                RootPanel.Children.Add(new TextBlock { Text = $"Texture coordinate count: {_mesh.TextureCoordinates.Count}" });

                if (_mesh.Positions.Count > 0)
                {
                    Rect3D bounds = new Rect3D(_mesh.Positions[0], new Size3D(0, 0, 0));
                    foreach (Point3D p in _mesh.Positions)
                    {
                        bounds.Union(p);
                    }
                    RootPanel.Children.Add(new TextBlock { Text = $"Bounding box: {bounds.SizeX:F2} x {bounds.SizeY:F2} x {bounds.SizeZ:F2}" });
                }
            }
        }

        private void Rebuild()
        {
            _mesh = BuildMesh();
            BuildViewport();
            BuildUvPreview();
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            VertexBuffer? vertexBuffer = _meshData.Table.Entries.OfType<VertexBuffer>().FirstOrDefault();
            IndiceData? indiceData = _meshData.Table.Entries.OfType<IndiceData>().FirstOrDefault();
            BuildInfoPanel(vertexBuffer, indiceData);
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
                Vector3D normal = new Vector3D(0, 0, 1);   // default

                foreach (VertexAttribute attr in decl.Value)
                {
                    switch (attr.Semantic)
                    {
                        case VertexAttributeSemantic.Position:
                            pos = ReadFloat3(data, ref offset);
                            break;
                        case VertexAttributeSemantic.Normal:
                            normal = ReadVector3(data, ref offset);
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
                    mesh.Normals.Add(normal);
                }
            }

            // Build index list
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

        private static Vector3D ReadVector3(byte[] data, ref int offset)
        {
            float x = BitConverter.ToSingle(data, offset); offset += 4;
            float y = BitConverter.ToSingle(data, offset); offset += 4;
            float z = BitConverter.ToSingle(data, offset); offset += 4;
            return new Vector3D(x, y, z);
        }

        private static Point ReadFloat2(byte[] data, ref int offset)
        {
            float u = BitConverter.ToSingle(data, offset); offset += 4;
            float v = BitConverter.ToSingle(data, offset); offset += 4;
            return new Point(u, 1.0 - v); // flip V for standard top‑left image origin
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the Export button text based on selected tab
            if (MainTabControl.SelectedItem is TabItem selectedTab)
            {
                string header = selectedTab.Header.ToString();
                if (header == "3D Preview")
                    ExportButton.Content = "Export Mesh as OBJ";
                else if (header == "UV Preview")
                    ExportButton.Content = "Export UV as PNG";
                else
                    ExportButton.Content = "Export";
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab)
            {
                string header = selectedTab.Header.ToString();
                if (header == "3D Preview")
                    ExportMeshAsObj();
                else if (header == "UV Preview")
                    ExportUvAsPng();
                else
                    MessageBox.Show("No export available for the Info tab.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab)
            {
                string header = selectedTab.Header.ToString();
                if (header == "3D Preview")
                    MessageBox.Show("Import from OBJ is not implemented yet.\n(Needs full vertex buffer layout first.)", "Import Mesh", MessageBoxButton.OK, MessageBoxImage.Information);
                else if (header == "UV Preview")
                    MessageBox.Show("Import UV from PNG is not implemented yet.\n(Would require reverse mapping to original vertex data.)", "Import UV", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Import is not available for the Info tab.", "Import", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportMeshAsObj()
        {
            if (_mesh == null || _mesh.Positions.Count == 0)
            {
                MessageBox.Show("No mesh data to export.", "Export Mesh", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "OBJ Files (*.obj)|*.obj",
                FileName = "mesh.obj"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(dialog.FileName, false, Encoding.ASCII))
            {
                writer.WriteLine("# Exported from Overlord Package Manager");
                writer.WriteLine($"# Vertices: {_mesh.Positions.Count}");
                writer.WriteLine($"# Triangles: {_mesh.TriangleIndices.Count / 3}");

                // Write vertices (v) – using InvariantCulture to force '.' as decimal separator
                System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.InvariantCulture;
                foreach (Point3D p in _mesh.Positions)
                    writer.WriteLine($"v {p.X.ToString(cult)} {p.Y.ToString(cult)} {p.Z.ToString(cult)}");

                // Write texture coordinates (vt)
                foreach (Point uv in _mesh.TextureCoordinates)
                {
                    writer.WriteLine($"vt {uv.X.ToString(cult)} {uv.Y.ToString(cult)}");
                }

                for (int i = 0; i < _mesh.TriangleIndices.Count; i += 3)
                {
                    int i1 = _mesh.TriangleIndices[i] + 1;
                    int i2 = _mesh.TriangleIndices[i + 1] + 1;
                    int i3 = _mesh.TriangleIndices[i + 2] + 1;
                    writer.WriteLine($"f {i1}/{i1} {i2}/{i2} {i3}/{i3}");
                }
            }

            MessageBox.Show($"Mesh exported to {dialog.FileName}", "Export Mesh", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportUvAsPng()
        {
            if (_uvPreview == null)
            {
                MessageBox.Show("No UV preview to export.", "Export UV", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            MessageBox.Show($"UV map exported to {dialog.FileName}", "Export UV", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}