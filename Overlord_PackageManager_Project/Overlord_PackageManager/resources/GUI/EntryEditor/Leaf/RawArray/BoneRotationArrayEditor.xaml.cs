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

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// View model for a single cluster item in the array
    /// </summary>
    public class BoneRotationItem
    {
        public string Label { get; set; } = string.Empty;
        public BoneRotationEditor? Editor { get; set; }
        public ObservableBoneRotation ObservableData { get; set; } = null!;
    }

    public partial class BoneRotationArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<BoneRotation[]>? _observableArray;
        private ObservableCollection<BoneRotationItem> _clusterItems = new();
        private bool _isUpdating;
        private const int ClusterSize = 36; // 9 floats * 4 bytes each    

        public BoneRotationArrayEditor()
        {
            InitializeComponent();
            ClustersItemsControl.ItemsSource = _clusterItems;
        }

        public BoneRotationArrayEditor(ObservableValue<BoneRotation[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Raw Mesh Cluster Data";
            set { }
        }

        private void BindToArray(ObservableValue<BoneRotation[]> array)
        {
            _observableArray = array;
            RebuildClusterItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<BoneRotation[]>.Value))
            {
                _isUpdating = true;
                RebuildClusterItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildClusterItems(BoneRotation[] clusters)
        {
            _clusterItems.Clear();

            for (int i = 0; i < clusters.Length; i++)
            {
                ObservableBoneRotation observableData = new ObservableBoneRotation(clusters[i]);

                // When the cluster changes, update the array
                observableData.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableBoneRotation.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                BoneRotationEditor editor = new BoneRotationEditor(observableData)
                {
                    Label = $"Cluster {i}"
                };

                BoneRotationItem item = new BoneRotationItem
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

            BoneRotation newCluster = new BoneRotation(0, 0, 0);
            BoneRotation[] newArray = new BoneRotation[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            newArray[^1] = newCluster;

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is BoneRotationItem item)
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

            foreach (BoneRotation cluster in _observableArray.Value)
            {
                BinaryTypes.BoneRotation.Write(writer, cluster);
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

            List<BoneRotation> list = new List<BoneRotation>();

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    BoneRotation cluster = BinaryTypes.BoneRotation.Read(reader);
                    list.Add(cluster);
                }
            }

            _observableArray.Value = list.ToArray();
        }
    }
}