using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    public partial class MatricesArrayEntryEditor : UserControl
    {
        private readonly RawArrayEntry<Matrix4x4> _entry;

        public MatricesArrayEntryEditor(RawArrayEntry<Matrix4x4> entry)
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

                Matrix4x4Editor editor = new(_entry.Value[index])
                {
                    Label = $"Matrix {index}"
                };

                editor.ValueChanged += (_, matrix) =>
                {
                    _entry.Value[index] = matrix;
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
                    List<Matrix4x4> matrices = _entry.Value.ToList();

                    matrices.RemoveAt(index);

                    _entry.Value = matrices.ToArray();

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
            List<Matrix4x4> matrices = _entry.Value.ToList();

            matrices.Add(Matrix4x4.Identity);

            _entry.Value = matrices.ToArray();

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

            foreach (Matrix4x4 matrix in _entry.Value)
            {
                BinaryTypes.Matrix4x4.Write(writer, matrix);
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

            if (fileInfo.Length % 64 != 0)
            {
                MessageBox.Show(
                    "File size is not a multiple of 64 bytes.\nA Matrix4x4 requires exactly 64 bytes.",
                    "Invalid File",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            List<Matrix4x4> matrices = [];

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    matrices.Add(BinaryTypes.Matrix4x4.Read(reader));
                }
            }
            _entry.Value = matrices.ToArray();

            RefreshEditors();
        }
    }
}