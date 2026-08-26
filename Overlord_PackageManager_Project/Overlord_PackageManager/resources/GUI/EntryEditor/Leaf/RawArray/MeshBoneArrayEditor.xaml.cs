using HelixToolkit.Maths;
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
    public class MeshBoneItem
    {
        public string Label { get; set; } = string.Empty;
        public MeshBoneEditor? Editor { get; set; }
        public ObservableMeshBone ObservableData { get; set; } = null!;
    }

    public partial class MeshBoneArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<MeshBone[]>? _observableArray;
        private ObservableCollection<MeshBoneItem> _clusterItems = new();
        private bool _isUpdating;

        public MeshBoneArrayEditor()
        {
            InitializeComponent();
            ClustersItemsControl.ItemsSource = _clusterItems;
        }

        public MeshBoneArrayEditor(ObservableValue<MeshBone[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Raw Mesh Cluster Data";
            set { }
        }

        private void BindToArray(ObservableValue<MeshBone[]> array)
        {
            _observableArray = array;
            RebuildClusterItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<MeshBone[]>.Value))
            {
                _isUpdating = true;
                RebuildClusterItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildClusterItems(MeshBone[] clusters)
        {
            _clusterItems.Clear();

            for (int i = 0; i < clusters.Length; i++)
            {
                ObservableMeshBone observableData = new ObservableMeshBone(clusters[i]);

                // When the cluster changes, update the array
                observableData.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableObjectBone.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                MeshBoneEditor editor = new MeshBoneEditor(observableData)
                {
                    Label = $"Cluster {i}"
                };

                MeshBoneItem item = new MeshBoneItem
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

            MeshBone newCluster = new MeshBone(
                new char[32], // Name
                new MeshTransform(
                    new Matrix4x4(
                        1, 0, 0, 0,
                        0, 1, 0, 0,
                        0, 0, 1, 0,
                        0, 0, 0, 1
                    ), // Matrix
                    new Vector3(1, 1, 1), // Scale
                    new Vector3(0, 0, 0),  // Translation
                    new Quaternion(0, 0, 0, 1) // Rotation
                ),
                0, // Unknown1
                0, // Unknown2
                0, // Unknown3
                0, // Unknown4
                0,   // Unknown5
                0  // Unknown6

            );
            MeshBone[] newArray = new MeshBone[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            newArray[^1] = newCluster;

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is MeshBoneItem item)
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

            foreach (MeshBone cluster in _observableArray.Value)
            {
                BinaryTypes.MeshBone.Write(writer, cluster);
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

            if (fileInfo.Length % BinaryTypes.MeshBone.Size != 0)
            {
                MessageBox.Show(
                    $"File size is not a multiple of {BinaryTypes.MeshBone.Size} bytes.",
                    "Invalid File",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            List<MeshBone> list = new List<MeshBone>();

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    MeshBone cluster = BinaryTypes.MeshBone.Read(reader);
                    list.Add(cluster);
                }
            }

            _observableArray.Value = list.ToArray();
        }
    }
}