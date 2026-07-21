using HelixToolkit;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using MediaColor = System.Windows.Media.Color;
using MediaVector3D = System.Windows.Media.Media3D.Vector3D;
using MeshGeometry3D = HelixToolkit.SharpDX.MeshGeometry3D;
using Pen = System.Windows.Media.Pen;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
using Point = System.Windows.Point;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Mesh
{
    public partial class MeshDataEditor : UserControl
    {
        private readonly MeshData _meshData;

        private MeshGeometry3D? _mesh;
        private BitmapSource? _uvPreview;

        private bool _disposed;

        public EffectsManager EffectsManager { get; } = new DefaultEffectsManager();

        public MeshDataEditor(MeshData meshData)
        {
            InitializeComponent();

            DataContext = this;

            EffectsManager = new DefaultEffectsManager();

            _meshData = meshData;

            Loaded += (_, __) => Build();
            Unloaded += (_, __) => Cleanup();
        }

        private void Cleanup()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            Viewport.Items.Clear();

            if (Viewport.EffectsManager is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Viewport.EffectsManager = null;
        }

        private void Build()
        {
            BuildMesh();
            BuildViewport();
            BuildUvPreview();
            BuildInfoPanel();
        }

        private void BuildMesh()
        {
            VertexBuffer? vertexBuffer = _meshData.Table.Entries.OfType<VertexBuffer>().FirstOrDefault();

            IndiceData? indiceData = _meshData.Table.Entries.OfType<IndiceData>().FirstOrDefault();

            if (vertexBuffer == null || indiceData == null)
            {
                return;
            }

            ByteArrayEntry? blob = vertexBuffer.Table.Entries.OfType<ByteArrayEntry>().FirstOrDefault();
            UInt32Entry? vertexCountEntry = vertexBuffer.Table.Entries.OfType<UInt32Entry>().FirstOrDefault(e => e.Id == 21);

            VertexBufferInfo? info = vertexBuffer.Table.Entries.OfType<VertexBufferInfo>().FirstOrDefault();
            UInt32Entry? strideEntry = info?.Table.Entries.OfType<UInt32Entry>().FirstOrDefault(e => e.Id == 21);
            VertexDeclarationEntry? decl = info?.Table.Entries.OfType<VertexDeclarationEntry>().FirstOrDefault();

            UInt16ArrayEntry? indicesEntry = indiceData.Table.Entries.OfType<UInt16ArrayEntry>().FirstOrDefault();

            if (blob?.Value == null || vertexCountEntry == null || strideEntry == null || decl == null || indicesEntry == null)
            {
                return;
            }

            int stride = (int)strideEntry.Value;
            int vertexCount = (int)vertexCountEntry.Value;

            byte[] data = blob.Value;

            Vector3Collection positions = new();
            Vector3Collection normals = new();
            Vector2Collection uvs = new();
            Vector3Collection tangents = new();
            Vector3Collection bitangents = new();
            IntCollection indices = new();

            for (int i = 0; i < vertexCount; i++)
            {
                int baseOffset = i * stride;
                if (baseOffset + stride > data.Length) break;

                int offset = baseOffset;

                Vector3 position = Vector3.Zero;
                Vector3 normal = Vector3.UnitZ;
                Vector2 uv = Vector2.Zero;
                Vector3 tangent = new Vector3(1, 0, 0);
                float handedness = 1.0f; // Default handedness

                foreach (VertexAttribute attr in decl.Value)
                {
                    switch (attr.Semantic)
                    {
                        case VertexAttributeSemantic.Position:
                            position = ReadVector3(data, ref offset);
                            break;
                        case VertexAttributeSemantic.Normal:
                            normal = ReadVector3(data, ref offset);
                            break;
                        case VertexAttributeSemantic.TexCoord:
                            uv = ReadVector2(data, ref offset);
                            break;
                        case VertexAttributeSemantic.TangentQuat:
                            Vector4 tangentQuat = ReadVector4(data, ref offset);
                            tangent = new Vector3(tangentQuat.X, tangentQuat.Y, tangentQuat.Z);
                            handedness = tangentQuat.W;
                            break;
                        default:
                            offset += attr.ByteSize;
                            break;
                    }
                }

                positions.Add(position);
                normals.Add(normal);
                uvs.Add(uv);
                tangents.Add(tangent);

                // --- CALCULATE BITANGENT HERE ---
                // Formula: B = (N x T) * Handedness
                Vector3 cross = Vector3.Cross(Vector3.Normalize(normal), Vector3.Normalize(tangent));
                Vector3 bitangent = cross * handedness;
                bitangents.Add(bitangent);
            }

            foreach (ushort idx in indicesEntry.Value)
            {
                if (idx < positions.Count) indices.Add(idx);
            }

            _mesh = new MeshGeometry3D
            {
                Positions = positions,
                Normals = normals,
                TextureCoordinates = uvs,
                Tangents = tangents,
                BiTangents = bitangents,
                Indices = indices
            };
        }

        private Element3D CreateGrid(int halfSize = 20, float spacing = 1.0f)
        {
            LineBuilder builder = new LineBuilder();

            for (int i = -halfSize; i <= halfSize; i++)
            {
                bool major = (i % 5) == 0;

                // Lines parallel to Z
                builder.AddLine(new Vector3(i * spacing, 0, -halfSize * spacing), new Vector3(i * spacing, 0, halfSize * spacing));

                // Lines parallel to X
                builder.AddLine(new Vector3(-halfSize * spacing, 0, i * spacing), new Vector3(halfSize * spacing, 0, i * spacing));
            }

            LineGeometry3D geometry = builder.ToLineGeometry3D();

            return new LineGeometryModel3D
            {
                Geometry = geometry,
                Color = MediaColor.FromArgb(90, 90, 90, 90),
                Thickness = 1.0
            };
        }

        private IEnumerable<Element3D> CreateAxes(int halfSize = 20, float spacing = 1.0f)
        {
            float extent = halfSize * spacing;

            // ---------- X AXIS (RED) ----------
            LineBuilder xBuilder = new LineBuilder();

            xBuilder.AddLine(new Vector3(-extent, 0.002f, 0), new Vector3(extent, 0.002f, 0));

            LineGeometryModel3D xAxis = new LineGeometryModel3D
            {
                Geometry = xBuilder.ToLineGeometry3D(),
                Color = Colors.Red,
                Thickness = 2.0
            };

            // ---------- Z AXIS (BLUE) ----------
            LineBuilder zBuilder = new LineBuilder();

            zBuilder.AddLine(new Vector3(0, 0.002f, -extent), new Vector3(0, 0.002f, extent));

            LineGeometryModel3D zAxis = new LineGeometryModel3D
            {
                Geometry = zBuilder.ToLineGeometry3D(),
                Color = Colors.Blue,
                Thickness = 2.0
            };

            return new Element3D[]
            {
                xAxis,
                zAxis
            };
        }

        private void BuildViewport()
        {
            Viewport.Items.Clear();

            // Ambient light
            Viewport.Items.Add(new AmbientLight3D
            {
                Color = MediaColor.FromRgb(90, 90, 90)
            });

            // Main directional light
            Viewport.Items.Add(new DirectionalLight3D
            {
                Color = Colors.White,
                Direction = new MediaVector3D(-1, -1, -1)
            });

            // Fill light
            Viewport.Items.Add(new DirectionalLight3D
            {
                Color = MediaColor.FromRgb(160, 160, 160),
                Direction = new MediaVector3D(1, -0.5f, 0.5f)
            });

            // Debug axis
            Viewport.Items.Add(new CoordinateSystemModel3D());
            Viewport.Items.Add(CreateGrid());
            foreach (Element3D axis in CreateAxes())
            {
                Viewport.Items.Add(axis);
            }

            if (_mesh == null)
            {
                return;
            }

            PhongMaterial material = new PhongMaterial
            {
                DiffuseColor = new Color4(0.85f, 0.85f, 0.85f, 1.0f),
                AmbientColor = new Color4(0.4f, 0.4f, 0.4f, 1.0f),
                SpecularColor = new Color4(1f, 1f, 1f, 1f),
                SpecularShininess = 16f
            };

            MeshGeometryModel3D model = new MeshGeometryModel3D
            {
                Geometry = _mesh,
                Material = material,
                CullMode = SharpDX.Direct3D11.CullMode.None
            };

            Viewport.Items.Add(model);

            CenterCameraOnMesh();
        }

        private void CenterCameraOnMesh()
        {
            if (_mesh == null || _mesh.Positions.Count == 0)
            {
                return;
            }

            Vector3 min = new Vector3(
                _mesh.Positions.Min(p => p.X),
                _mesh.Positions.Min(p => p.Y),
                _mesh.Positions.Min(p => p.Z));

            Vector3 max = new Vector3(
                _mesh.Positions.Max(p => p.X),
                _mesh.Positions.Max(p => p.Y),
                _mesh.Positions.Max(p => p.Z));

            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;

            float radius = Math.Max(size.X, Math.Max(size.Y, size.Z));

            float distance = radius * 2.5f;

            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(
                    center.X + distance,
                    center.Y + distance,
                    center.Z + distance),

                LookDirection = new Vector3D(
                    -distance,
                    -distance,
                    -distance),

                UpDirection = new Vector3D(0, 1, 0),

                FarPlaneDistance = 100000
            };

            Viewport.Camera = camera;
        }

        private void BuildInfoPanel()
        {
            RootPanel.Children.Clear();

            if (_mesh == null)
            {
                RootPanel.Children.Add(new TextBlock
                {
                    Text = "Mesh not available",
                    Foreground = Brushes.Red
                });

                return;
            }

            RootPanel.Children.Add(new TextBlock
            {
                Text = $"Vertices: {_mesh.Positions.Count}",
                FontWeight = FontWeights.Bold
            });

            RootPanel.Children.Add(new TextBlock
            {
                Text = $"Triangles: {_mesh.Indices.Count / 3}"
            });

            RootPanel.Children.Add(new TextBlock
            {
                Text = $"UVs: {_mesh.TextureCoordinates.Count}"
            });

            RootPanel.Children.Add(new TextBlock
            {
                Text = $"Tangents: {_mesh.Tangents?.Count ?? 0}"
            });

            RootPanel.Children.Add(new TextBlock
            {
                Text = $"Bitangents: {_mesh.BiTangents?.Count ?? 0}"
            });

            if (_mesh.Tangents != null && _mesh.Tangents.Count > 0)
            {
                Vector3 t = _mesh.Tangents[0];

                RootPanel.Children.Add(new TextBlock
                {
                    Text = $"First Tangent: ({t.X:F3}, {t.Y:F3}, {t.Z:F3})"
                });
            }

            if (_mesh.BiTangents != null && _mesh.BiTangents.Count > 0)
            {
                Vector3 b = _mesh.BiTangents[0];
                RootPanel.Children.Add(new TextBlock
                {
                    Text = $"First Bitangent: ({b.X:F3}, {b.Y:F3}, {b.Z:F3})"
                });
            }
        }

        private void BuildUvPreview()
        {
            if (_mesh == null || _mesh.TextureCoordinates == null || _mesh.TextureCoordinates.Count == 0 || _mesh.Indices == null || _mesh.Indices.Count == 0)
            {
                ShowPlaceholder("No UV data");
                return;
            }

            const int size = 1024;

            float minU = float.MaxValue;
            float minV = float.MaxValue;

            float maxU = float.MinValue;
            float maxV = float.MinValue;

            foreach (Vector2 uv in _mesh.TextureCoordinates)
            {
                minU = Math.Min(minU, uv.X);
                minV = Math.Min(minV, uv.Y);
                maxU = Math.Max(maxU, uv.X);
                maxV = Math.Max(maxV, uv.Y);
            }

            float rangeU = maxU - minU;
            float rangeV = maxV - minV;

            if (Math.Abs(rangeU) < 1e-6f)
            {
                rangeU = 1.0f;
            }

            if (Math.Abs(rangeV) < 1e-6f)
            {
                rangeV = 1.0f;
            }

            float margin = 0.1f;

            minU -= rangeU * margin;
            maxU += rangeU * margin;
            minV -= rangeV * margin;
            maxV += rangeV * margin;

            Point ToPixel(Vector2 uv)
            {
                double nx = (uv.X - minU) / (maxU - minU);
                double ny = 1.0 - ((uv.Y - minV) / (maxV - minV));

                return new Point(
                    nx * (size - 1),
                    ny * (size - 1));
            }

            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                DrawCheckerboard(dc, size);

                Pen pen = new Pen(Brushes.White, 2.0);

                SolidColorBrush fill =
                    new SolidColorBrush(
                        MediaColor.FromArgb(120, 255, 255, 0));

                for (int i = 0; i < _mesh.Indices.Count; i += 3)
                {
                    int i0 = _mesh.Indices[i];
                    int i1 = _mesh.Indices[i + 1];
                    int i2 = _mesh.Indices[i + 2];

                    if (i0 >= _mesh.TextureCoordinates.Count ||
                        i1 >= _mesh.TextureCoordinates.Count ||
                        i2 >= _mesh.TextureCoordinates.Count)
                    {
                        continue;
                    }

                    Point p0 = ToPixel(_mesh.TextureCoordinates[i0]);
                    Point p1 = ToPixel(_mesh.TextureCoordinates[i1]);
                    Point p2 = ToPixel(_mesh.TextureCoordinates[i2]);

                    // Skip degenerate triangles
                    if (Math.Abs(p0.X - p1.X) < 0.5 &&
                        Math.Abs(p0.Y - p1.Y) < 0.5 &&
                        Math.Abs(p0.X - p2.X) < 0.5 &&
                        Math.Abs(p0.Y - p2.Y) < 0.5)
                    {
                        continue;
                    }

                    StreamGeometry geo = new StreamGeometry();

                    using (StreamGeometryContext ctx = geo.Open())
                    {
                        ctx.BeginFigure(p0, true, true);
                        ctx.LineTo(p1, true, false);
                        ctx.LineTo(p2, true, false);
                    }

                    geo.Freeze();

                    dc.DrawGeometry(fill, pen, geo);
                }
            }

            RenderTargetBitmap bmp =
                new RenderTargetBitmap(
                    size,
                    size,
                    96,
                    96,
                    PixelFormats.Pbgra32);

            bmp.Render(drawingVisual);
            bmp.Freeze();

            _uvPreview = bmp;
            UvImage.Source = bmp;
        }

        private static void DrawCheckerboard(DrawingContext dc, int size)
        {
            int tiles = 8;
            double tileSize = (double)size / tiles;

            Brush dark = new SolidColorBrush(MediaColor.FromRgb(30, 30, 30));
            Brush light = new SolidColorBrush(MediaColor.FromRgb(70, 70, 70));

            for (int y = 0; y < tiles; y++)
            {
                for (int x = 0; x < tiles; x++)
                {
                    Brush tileColor;

                    if ((x + y) % 2 == 0)
                    {
                        tileColor = dark;
                    }
                    else
                    {
                        tileColor = light;
                    }

                    Rect rectangleBounds = new Rect(x * tileSize, y * tileSize, tileSize, tileSize);
                    dc.DrawRectangle(tileColor, null, rectangleBounds);
                }
            }
        }

        private static void ShowMessage(string text)
        {
            MessageBox.Show(text, "Mesh Editor", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowPlaceholder(string message)
        {
            DrawingVisual visual = new DrawingVisual();

            using DrawingContext dc = visual.RenderOpen();

            dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, 512, 256));

            FormattedText text = new FormattedText(
                message,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                16,
                Brushes.White,
                96);

            dc.DrawText(text, new Point(20, 100));

            RenderTargetBitmap bmp = new RenderTargetBitmap(512, 256, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(visual);
            bmp.Freeze();

            _uvPreview = bmp;
            UvImage.Source = bmp;
        }

        private static Vector3 ReadVector3(byte[] data, ref int offset)
        {
            float x = BitConverter.ToSingle(data, offset); offset += 4;
            float y = BitConverter.ToSingle(data, offset); offset += 4;
            float z = BitConverter.ToSingle(data, offset); offset += 4;

            return new Vector3(x, y, z);
        }

        private static Vector2 ReadVector2(byte[] data, ref int offset)
        {
            float x = BitConverter.ToSingle(data, offset); offset += 4;
            float y = BitConverter.ToSingle(data, offset); offset += 4;

            return new Vector2(x, y);
        }

        private static Vector4 ReadVector4(byte[] data, ref int offset)
        {
            float x = BitConverter.ToSingle(data, offset); offset += 4;
            float y = BitConverter.ToSingle(data, offset); offset += 4;
            float z = BitConverter.ToSingle(data, offset); offset += 4;
            float w = BitConverter.ToSingle(data, offset); offset += 4;

            return new Vector4(x, y, z, w);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedItem is not TabItem tab)
            {
                return;
            }

            string header = tab.Header.ToString() ?? string.Empty;

            ExportButton.Content = header switch
            {
                "3D Preview" => "Export Mesh as OBJ",
                "UV Preview" => "Export UV as PNG",
                _ => "Export"
            };
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is not TabItem tab)
            {
                return;
            }

            string header = tab.Header.ToString() ?? string.Empty;

            switch (header)
            {
                case "3D Preview":
                    ExportMeshAsObj();
                    break;

                case "UV Preview":
                    ExportUvAsPng();
                    break;

                default:
                    ShowMessage("No export available.");
                    break;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("Import not implemented yet.");
        }

        private void ExportMeshAsObj()
        {
            if (_mesh == null)
            {
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

                System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.InvariantCulture;

                foreach (Vector3 p in _mesh.Positions)
                {
                    writer.WriteLine($"v {p.X.ToString(cult)} {p.Y.ToString(cult)} {p.Z.ToString(cult)}");
                }

                foreach (Vector2 uv in _mesh.TextureCoordinates)
                {
                    writer.WriteLine($"vt {uv.X.ToString(cult)} {uv.Y.ToString(cult)}");
                }

                foreach (Vector3 n in _mesh.Normals)
                {
                    writer.WriteLine($"vn {n.X.ToString(cult)} {n.Y.ToString(cult)} {n.Z.ToString(cult)}");
                }

                for (int i = 0; i < _mesh.Indices.Count; i += 3)
                {
                    int i1 = _mesh.Indices[i] + 1;
                    int i2 = _mesh.Indices[i + 1] + 1;
                    int i3 = _mesh.Indices[i + 2] + 1;

                    writer.WriteLine($"f {i1}/{i1}/{i1} {i2}/{i2}/{i2} {i3}/{i3}/{i3}");
                }
            }
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