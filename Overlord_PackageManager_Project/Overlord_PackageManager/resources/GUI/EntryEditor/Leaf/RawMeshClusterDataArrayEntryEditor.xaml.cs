using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Generic;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class RawMeshClusterDataArrayEntryEditor : UserControl
    {
        private readonly RawMeshClusterDataArrayEntry _entry;

        private const int ClusterSize = 64;

        public RawMeshClusterDataArrayEntryEditor(RawMeshClusterDataArrayEntry entry)
        {
            InitializeComponent();

            _entry = entry;

            if (_entry.Value == null)
            {
                _entry.Value = [];
            }

            RefreshEditors();
        }

        private void RefreshEditors()
        {
            EditorContainer.Children.Clear();

            for (int i = 0; i < _entry.Value.Length; i++)
            {
                int index = i;

                RawMeshClusterDataEditor editor = new(_entry.Value[index])
                {
                    Label = $"Cluster {index}"
                };

                editor.ValueChanged += (_, value) =>
                {
                    _entry.Value[index] = value;
                };

                Grid container = new();

                Border border = new()
                {
                    Padding = new Thickness(5),
                    BorderThickness = new Thickness(1),
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    CornerRadius = new CornerRadius(4),
                    Child = editor
                };

                container.Children.Add(border);

                Button removeButton = new()
                {
                    Content = "X",
                    Width = 24,
                    Height = 24,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 5, 5, 0),
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.DarkRed,
                    Foreground = Brushes.White,
                    BorderBrush = Brushes.Firebrick
                };

                removeButton.Click += (_, _) =>
                {
                    List<RawMeshClusterData> list = _entry.Value.ToList();

                    list.RemoveAt(index);

                    _entry.Value = list.ToArray();

                    RefreshEditors();
                };

                container.Children.Add(removeButton);

                EditorContainer.Children.Add(new Border
                {
                    Margin = new Thickness(0, 0, 0, 10),
                    Child = container
                });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<RawMeshClusterData> list = _entry.Value.ToList();

            list.Add(new RawMeshClusterData(
                new Matrix3x3(
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1),
                Vector3.Zero,
                Vector3.Zero,
                0,
                0));

            _entry.Value = list.ToArray();

            RefreshEditors();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new()
            {
                Filter = "Binary Files (*.bin)|*.bin"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            using BinaryWriter writer = new(File.Create(dialog.FileName));

            foreach (RawMeshClusterData cluster in _entry.Value)
            {
                writer.Write(cluster.Matrix.M11);
                writer.Write(cluster.Matrix.M12);
                writer.Write(cluster.Matrix.M13);

                writer.Write(cluster.Matrix.M21);
                writer.Write(cluster.Matrix.M22);
                writer.Write(cluster.Matrix.M23);

                writer.Write(cluster.Matrix.M31);
                writer.Write(cluster.Matrix.M32);
                writer.Write(cluster.Matrix.M33);

                writer.Write(cluster.Center.X);
                writer.Write(cluster.Center.Y);
                writer.Write(cluster.Center.Z);

                writer.Write(cluster.Extents.X);
                writer.Write(cluster.Extents.Y);
                writer.Write(cluster.Extents.Z);

                writer.Write(cluster.patchIndex);
                writer.Write(cluster.triangleCount);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            FileInfo fileInfo = new(dialog.FileName);

            if (fileInfo.Length % ClusterSize != 0)
            {
                MessageBox.Show(
                    $"File size is not a multiple of {ClusterSize} bytes.",
                    "Invalid File",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            List<RawMeshClusterData> list = [];

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    Matrix3x3 matrix = new(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),

                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),

                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle());

                    Vector3 center = new(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle());

                    Vector3 extents = new(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle());

                    ushort patchIndex = reader.ReadUInt16();
                    ushort triangleCount = reader.ReadUInt16();

                    list.Add(new RawMeshClusterData(
                        matrix,
                        center,
                        extents,
                        patchIndex,
                        triangleCount));
                }
            }

            _entry.Value = list.ToArray();

            RefreshEditors();
        }
    }
}