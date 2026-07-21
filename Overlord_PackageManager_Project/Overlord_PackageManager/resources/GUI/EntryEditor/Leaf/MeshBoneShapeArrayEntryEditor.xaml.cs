using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class MeshBoneShapeArrayEntryEditor : UserControl
    {
        private readonly MeshBoneShapeArrayEntry _entry;

        public MeshBoneShapeArrayEntryEditor(MeshBoneShapeArrayEntry entry)
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

                MeshBoneShapeEditor editor = new(_entry.Value[index])
                {
                    Label = $"Bone Shape {index}"
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
                    List<MeshBoneShape> list = _entry.Value.ToList();

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
            List<MeshBoneShape> list = _entry.Value.ToList();

            list.Add(new MeshBoneShape(
                new Matrix3x3(
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1),
                Vector3.Zero,
                Vector3.Zero));

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

            foreach (MeshBoneShape shape in _entry.Value)
            {
                writer.Write(shape.Matrix.M11);
                writer.Write(shape.Matrix.M12);
                writer.Write(shape.Matrix.M13);

                writer.Write(shape.Matrix.M21);
                writer.Write(shape.Matrix.M22);
                writer.Write(shape.Matrix.M23);

                writer.Write(shape.Matrix.M31);
                writer.Write(shape.Matrix.M32);
                writer.Write(shape.Matrix.M33);

                writer.Write(shape.Head.X);
                writer.Write(shape.Head.Y);
                writer.Write(shape.Head.Z);

                writer.Write(shape.Tail.X);
                writer.Write(shape.Tail.Y);
                writer.Write(shape.Tail.Z);
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

            if (fileInfo.Length % 60 != 0)
            {
                MessageBox.Show(
                    "File size is not a multiple of 60 bytes.",
                    "Invalid File",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            List<MeshBoneShape> list = [];

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

                    Vector3 head = new(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle());

                    Vector3 tail = new(
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle());

                    list.Add(new MeshBoneShape(
                        matrix,
                        head,
                        tail));
                }
            }

            _entry.Value = list.ToArray();

            RefreshEditors();
        }
    }
}