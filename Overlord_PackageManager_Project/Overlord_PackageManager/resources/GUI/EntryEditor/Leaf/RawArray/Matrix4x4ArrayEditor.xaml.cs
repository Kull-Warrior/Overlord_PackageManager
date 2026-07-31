using HelixToolkit.Maths;
using Microsoft.Win32;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// View model for a single cluster item in the array
    /// </summary>
    public class Matrix4x4Item
    {
        public string Label { get; set; } = string.Empty;
        public Matrix4x4Editor? Editor { get; set; }
        public ObservableMatrix4x4 ObservableData { get; set; } = null!;
    }

    public partial class Matrix4x4ArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<Matrix4x4[]>? _observableArray;
        private ObservableCollection<Matrix4x4Item> _clusterItems = new();
        private bool _isUpdating;
        private const int ClusterSize = 64; // 16 floats * 4 bytes each  

        public Matrix4x4ArrayEditor()
        {
            InitializeComponent();
            ClustersItemsControl.ItemsSource = _clusterItems;
        }

        public Matrix4x4ArrayEditor(ObservableValue<Matrix4x4[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Raw Mesh Cluster Data";
            set { }
        }

        private void BindToArray(ObservableValue<Matrix4x4[]> array)
        {
            _observableArray = array;
            RebuildClusterItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<Matrix4x4[]>.Value))
            {
                _isUpdating = true;
                RebuildClusterItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildClusterItems(Matrix4x4[] clusters)
        {
            _clusterItems.Clear();

            for (int i = 0; i < clusters.Length; i++)
            {
                ObservableMatrix4x4 observableData = new ObservableMatrix4x4(clusters[i]);

                // When the cluster changes, update the array
                observableData.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableMatrix4x4.Matrix))
                    {
                        UpdateArrayFromItems();
                    }
                };

                Matrix4x4Editor editor = new Matrix4x4Editor(observableData)
                {
                    Label = $"Cluster {i}"
                };

                Matrix4x4Item item = new Matrix4x4Item
                {
                    Label = $"Cluster {i}",
                    Editor = editor,
                    ObservableData = observableData
                };

                _clusterItems.Add(item);
            }
        }

        private void UpdateArrayFromItems()
        {
            if (_isUpdating || _observableArray == null) return;

            _isUpdating = true;
            _observableArray.Value = _clusterItems.Select(item => item.ObservableData.Matrix).ToArray();
            _isUpdating = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            Matrix4x4 newMatrix = new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
            Matrix4x4[] newArray = new Matrix4x4[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            newArray[^1] = newMatrix;

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Matrix4x4Item item)
            {
                _clusterItems.Remove(item);
                UpdateArrayFromItems();
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            SaveFileDialog dialog = new()
            {
                Filter = "Binary Files (*.bin)|*.bin"
            };

            if (dialog.ShowDialog() != true) return;

            using BinaryWriter writer = new(File.Create(dialog.FileName));

            foreach (Matrix4x4 matrix in _observableArray.Value)
            {
                writer.Write(matrix.M11);
                writer.Write(matrix.M12);
                writer.Write(matrix.M13);
                writer.Write(matrix.M14);
                writer.Write(matrix.M21);
                writer.Write(matrix.M22);
                writer.Write(matrix.M23);
                writer.Write(matrix.M24);
                writer.Write(matrix.M31);
                writer.Write(matrix.M32);
                writer.Write(matrix.M33);
                writer.Write(matrix.M34);
                writer.Write(matrix.M41);
                writer.Write(matrix.M42);
                writer.Write(matrix.M43);
                writer.Write(matrix.M44);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            OpenFileDialog dialog = new()
            {
                Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true) return;

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

            List<Matrix4x4> list = new List<Matrix4x4>();

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    Matrix4x4 matrix = new(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    list.Add(matrix);
                }
            }

            _observableArray.Value = list.ToArray();
        }
    }
}