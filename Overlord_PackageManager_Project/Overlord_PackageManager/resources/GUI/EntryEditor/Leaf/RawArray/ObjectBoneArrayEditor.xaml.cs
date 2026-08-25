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
    public class ObjectBoneItem
    {
        public string Label { get; set; } = string.Empty;
        public ObjectBoneEditor? Editor { get; set; }
        public ObservableObjectBone ObservableData { get; set; } = null!;
    }

    public partial class ObjectBoneArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<ObjectBone[]>? _observableArray;
        private ObservableCollection<ObjectBoneItem> _clusterItems = new();
        private bool _isUpdating;
        private const int ClusterSize = 32 + (16 * 4) + (4 * 4) + (4 * 3) + (4 * 3) + (4 * 3) + (4 * 3) + (4 * 1); // Size of ObjectBone in bytes   

        public ObjectBoneArrayEditor()
        {
            InitializeComponent();
            ClustersItemsControl.ItemsSource = _clusterItems;
        }

        public ObjectBoneArrayEditor(ObservableValue<ObjectBone[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Raw Mesh Cluster Data";
            set { }
        }

        private void BindToArray(ObservableValue<ObjectBone[]> array)
        {
            _observableArray = array;
            RebuildClusterItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<ObjectBone[]>.Value))
            {
                _isUpdating = true;
                RebuildClusterItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildClusterItems(ObjectBone[] clusters)
        {
            _clusterItems.Clear();

            for (int i = 0; i < clusters.Length; i++)
            {
                ObservableObjectBone observableData = new ObservableObjectBone(clusters[i]);

                // When the cluster changes, update the array
                observableData.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableObjectBone.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                ObjectBoneEditor editor = new ObjectBoneEditor(observableData)
                {
                    Label = $"Cluster {i}"
                };

                ObjectBoneItem item = new ObjectBoneItem
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

            ObjectBone newCluster = new ObjectBone(
                new char[32], // Name
                new Transform(
                    new Matrix4x4(
                        1, 0, 0, 0,
                        0, 1, 0, 0,
                        0, 0, 1, 0,
                        0, 0, 0, 1
                    ), // Matrix
                    new Quaternion(0, 0, 0, 0), // Rotation
                    new Vector3(0, 0, 0)  // Translation
                ),
                -1, // SkinID
                -1, // ParentIndex
                -1, // NextSiblingIndex
                -1, // FirstChildIndex
                0   // Reserved

            );
            ObjectBone[] newArray = new ObjectBone[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            newArray[^1] = newCluster;

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ObjectBoneItem item)
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

            foreach (ObjectBone cluster in _observableArray.Value)
            {
                writer.Write(cluster.Name);
                writer.Write(cluster.Transform.Matrix.M11);
                writer.Write(cluster.Transform.Matrix.M12);
                writer.Write(cluster.Transform.Matrix.M13);
                writer.Write(cluster.Transform.Matrix.M14);
                writer.Write(cluster.Transform.Matrix.M21);
                writer.Write(cluster.Transform.Matrix.M22);
                writer.Write(cluster.Transform.Matrix.M23);
                writer.Write(cluster.Transform.Matrix.M24);
                writer.Write(cluster.Transform.Matrix.M31);
                writer.Write(cluster.Transform.Matrix.M32);
                writer.Write(cluster.Transform.Matrix.M33);
                writer.Write(cluster.Transform.Matrix.M34);
                writer.Write(cluster.Transform.Matrix.M41);
                writer.Write(cluster.Transform.Matrix.M42);
                writer.Write(cluster.Transform.Matrix.M43);
                writer.Write(cluster.Transform.Matrix.M44);
                writer.Write(cluster.Transform.Rotation.X);
                writer.Write(cluster.Transform.Rotation.Y);
                writer.Write(cluster.Transform.Rotation.Z);
                writer.Write(cluster.Transform.Rotation.W);
                writer.Write(cluster.Transform.Translation.X);
                writer.Write(cluster.Transform.Translation.Y);
                writer.Write(cluster.Transform.Translation.Z);
                writer.Write(cluster.SkinID);
                writer.Write(cluster.ParentIndex);
                writer.Write(cluster.NextSiblingIndex);
                writer.Write(cluster.FirstChildIndex);
                writer.Write(cluster.Reserved);
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

            List<ObjectBone> list = new List<ObjectBone>();

            using (BinaryReader reader = new(File.OpenRead(dialog.FileName)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    char[] name = reader.ReadChars(32);

                    Transform transform = new Transform
                    (
                        new Matrix4x4(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()
                        ),
                        new Quaternion(
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle()
                        ),
                        new Vector3(
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle()
                        )
                    );

                    int skinID = reader.ReadInt32();
                    int parentIndex = reader.ReadInt32();
                    int nextSiblingIndex = reader.ReadInt32();
                    int firstChildIndex = reader.ReadInt32();
                    int reserved = reader.ReadInt32();

                    list.Add(new ObjectBone
                    (
                        name,
                        transform,
                        skinID,
                        parentIndex,
                        nextSiblingIndex,
                        firstChildIndex,
                        reserved
                    ));
                }
            }

            _observableArray.Value = list.ToArray();
        }
    }
}