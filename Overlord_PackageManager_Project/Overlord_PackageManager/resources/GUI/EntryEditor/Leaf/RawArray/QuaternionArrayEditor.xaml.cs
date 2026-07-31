using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// View model for a single Quaternion item in the array
    /// </summary>
    public class QuaternionArrayItem
    {
        public QuaternionEditor? Editor { get; set; }
        public ObservableQuaternion ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for UInt16ArrayEditor.xaml
    /// </summary>
    public partial class QuaternionArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<Quaternion[]>? _observableArray;
        private ObservableCollection<QuaternionArrayItem> _items = new();
        private bool _isUpdating;

        public QuaternionArrayEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public QuaternionArrayEditor(ObservableValue<Quaternion[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Quaternion Array";
            set { }
        }

        private void BindToArray(ObservableValue<Quaternion[]> array)
        {
            _observableArray = array;
            RebuildItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<Quaternion[]>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(Quaternion[] values)
        {
            _items.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                ObservableQuaternion observableValue = new ObservableQuaternion(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableQuaternion.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                QuaternionEditor editor = new QuaternionEditor(observableValue)
                {
                    Label = $"[{i}]"
                };

                QuaternionArrayItem item = new QuaternionArrayItem
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

            Quaternion[] newArray = new Quaternion[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            // newArray[^1] is already 0 (default)

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Length == 0) return;

            Quaternion[] newArray = new Quaternion[_observableArray.Value.Length - 1];
            Array.Copy(_observableArray.Value, newArray, newArray.Length);

            _observableArray.Value = newArray;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is QuaternionArrayItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}