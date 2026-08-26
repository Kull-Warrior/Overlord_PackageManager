using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// View model for a single MeshTransform item in the array
    /// </summary>
    public class MeshTransformArrayItem
    {
        public MeshTransformEditor? Editor { get; set; }
        public ObservableMeshTransform ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for UInt16ArrayEditor.xaml
    /// </summary>
    public partial class MeshTransformArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<MeshTransform[]>? _observableArray;
        private ObservableCollection<MeshTransformArrayItem> _items = new();
        private bool _isUpdating;

        public MeshTransformArrayEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public MeshTransformArrayEditor(ObservableValue<MeshTransform[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Mesh Transform Array";
            set { }
        }

        private void BindToArray(ObservableValue<MeshTransform[]> array)
        {
            _observableArray = array;
            RebuildItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<MeshTransform[]>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(MeshTransform[] values)
        {
            _items.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                ObservableMeshTransform observableValue = new ObservableMeshTransform(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableMeshTransform.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                MeshTransformEditor editor = new MeshTransformEditor(observableValue)
                {
                    Label = $"[{i}]"
                };

                MeshTransformArrayItem item = new MeshTransformArrayItem
                {
                    Editor = editor,
                    ObservableValue = observableValue
                };

                _items.Add(item);
            }

            UpdateCountDisplay();
        }

        private void UpdateArrayFromItems()
        {
            if (_isUpdating || _observableArray == null) return;

            _isUpdating = true;
            _observableArray.Value = _items.Select(item => item.ObservableValue.Value).ToArray();
            _isUpdating = false;
        }

        private void UpdateCountDisplay()
        {
            CountBlock.Text = $"Count: {_items.Count}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            MeshTransform[] newArray = new MeshTransform[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            // newArray[^1] is already 0 (default)

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Length == 0) return;

            MeshTransform[] newArray = new MeshTransform[_observableArray.Value.Length - 1];
            Array.Copy(_observableArray.Value, newArray, newArray.Length);

            _observableArray.Value = newArray;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is MeshTransformArrayItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}