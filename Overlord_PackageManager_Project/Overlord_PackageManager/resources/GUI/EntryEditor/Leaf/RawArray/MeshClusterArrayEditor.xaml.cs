using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.DataTypes;
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
    public class ClusterItem
    {
        public string Label { get; set; } = string.Empty;
        public MeshClusterEditor? Editor { get; set; }
        public ObservableMeshCluster ObservableData { get; set; } = null!;
    }

    public partial class MeshClusterArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<MeshCluster[]>? _observableArray;
        private ObservableCollection<ClusterItem> _clusterItems = new();
        private bool _isUpdating;
        private const int ClusterSize = 64; // Matrix(36) + Vector3(12) + Vector3(12) + ushort(2) + ushort(2) = 64

        public MeshClusterArrayEditor()
        {
            InitializeComponent();
            ClustersItemsControl.ItemsSource = _clusterItems;
        }

        public MeshClusterArrayEditor(ObservableValue<MeshCluster[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Raw Mesh Cluster Data";
            set { }
        }

        private void BindToArray(ObservableValue<MeshCluster[]> array)
        {
            _observableArray = array;
            RebuildClusterItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<MeshCluster[]>.Value))
            {
                _isUpdating = true;
                RebuildClusterItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildClusterItems(MeshCluster[] clusters)
        {
            _clusterItems.Clear();

            for (int i = 0; i < clusters.Length; i++)
            {
                ObservableMeshCluster observableData = new ObservableMeshCluster(clusters[i]);

                // When the cluster changes, update the array
                observableData.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableMeshCluster.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                MeshClusterEditor editor = new MeshClusterEditor(observableData)
                {
                    Label = $"Cluster {i}"
                };

                ClusterItem item = new ClusterItem
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

            MeshCluster newCluster = new MeshCluster(
                new Matrix3x3(
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1),
                Vector3.Zero,
                Vector3.Zero,
                0,
                0);

            MeshCluster[] newArray = new MeshCluster[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            newArray[^1] = newCluster;

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ClusterItem item)
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

            foreach (MeshCluster cluster in _observableArray.Value)
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
                writer.Write(cluster.PatchIndex);
                writer.Write(cluster.TriangleCount);
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

            List<MeshCluster> list = [];

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    Matrix3x3 matrix = new(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    Vector3 center = new(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    Vector3 extents = new(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    ushort patchIndex = reader.ReadUInt16();
                    ushort triangleCount = reader.ReadUInt16();

                    list.Add(new MeshCluster(matrix, center, extents, patchIndex, triangleCount));
                }
            }

            _observableArray.Value = list.ToArray();
        }
    }
}