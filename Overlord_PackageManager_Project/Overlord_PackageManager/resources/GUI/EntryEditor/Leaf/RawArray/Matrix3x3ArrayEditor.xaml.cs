using HelixToolkit.Maths;
using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Matrix3x3 = Overlord_PackageManager.resources.Data.DataTypes.Matrix3x3;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// View model for a single cluster item in the array
    /// </summary>
    public class Matrix3x3Item
    {
        public string Label { get; set; } = string.Empty;
        public Matrix3x3Editor? Editor { get; set; }
        public ObservableMatrix3x3 ObservableData { get; set; } = null!;
    }

    public partial class Matrix3x3ArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<Matrix3x3[]>? _observableArray;
        private ObservableCollection<Matrix3x3Item> _clusterItems = new();
        private bool _isUpdating;
        private const int ClusterSize = 36; // 9 floats * 4 bytes each    

        public Matrix3x3ArrayEditor()
        {
            InitializeComponent();
            ClustersItemsControl.ItemsSource = _clusterItems;
        }

        public Matrix3x3ArrayEditor(ObservableValue<Matrix3x3[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Raw Mesh Cluster Data";
            set { }
        }

        private void BindToArray(ObservableValue<Matrix3x3[]> array)
        {
            _observableArray = array;
            RebuildClusterItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<Matrix3x3[]>.Value))
            {
                _isUpdating = true;
                RebuildClusterItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildClusterItems(Matrix3x3[] clusters)
        {
            _clusterItems.Clear();

            for (int i = 0; i < clusters.Length; i++)
            {
                ObservableMatrix3x3 observableData = new ObservableMatrix3x3(clusters[i]);

                // When the cluster changes, update the array
                observableData.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableMatrix3x3.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                Matrix3x3Editor editor = new Matrix3x3Editor(observableData)
                {
                    Label = $"Cluster {i}"
                };

                Matrix3x3Item item = new Matrix3x3Item
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
            _observableArray.Value = _clusterItems
                .Select(item => item.ObservableData.Value)
                .ToArray();
            _isUpdating = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            Matrix3x3 newCluster = new Matrix3x3(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1
            );
            Matrix3x3[] newArray = new Matrix3x3[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            newArray[^1] = newCluster;

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Matrix3x3Item item)
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

            foreach (Matrix3x3 cluster in _observableArray.Value)
            {
                BinaryTypes.Matrix3x3.Write(writer, cluster);
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

            List<Matrix3x3> list = new List<Matrix3x3>();

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    Matrix3x3 matrix = BinaryTypes.Matrix3x3.Read(reader);
                    list.Add(matrix);
                }
            }

            _observableArray.Value = list.ToArray();
        }
    }
}