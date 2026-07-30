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
    /// View model for a single long item in the array
    /// </summary>
    public class Int64ArrayItem
    {
        public Int64Editor? Editor { get; set; }
        public ObservableValue<long> ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for Int64ArrayEditor.xaml
    /// </summary>
    public partial class Int64ArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<long[]>? _observableArray;
        private ObservableCollection<Int64ArrayItem> _items = new();
        private bool _isUpdating;

        public Int64ArrayEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public Int64ArrayEditor(ObservableValue<long[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Int64 Array";
            set { }
        }

        private void BindToArray(ObservableValue<long[]> array)
        {
            _observableArray = array;
            RebuildItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<long[]>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(long[] values)
        {
            _items.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                ObservableValue<long> observableValue = new ObservableValue<long>(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableValue<long>.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                Int64Editor editor = new Int64Editor(observableValue)
                {
                    Label = $"[{i}]"
                };

                Int64ArrayItem item = new Int64ArrayItem
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

            long[] newArray = new long[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            // newArray[^1] is already 0 (default)

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Length == 0) return;

            long[] newArray = new long[_observableArray.Value.Length - 1];
            Array.Copy(_observableArray.Value, newArray, newArray.Length);

            _observableArray.Value = newArray;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Int64ArrayItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}