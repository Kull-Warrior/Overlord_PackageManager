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
    /// View model for a single int item in the array
    /// </summary>
    public class Int32ArrayItem
    {
        public Int32Editor? Editor { get; set; }
        public ObservableValue<int> ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for Int32ArrayEditor.xaml
    /// </summary>
    public partial class Int32ArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<int[]>? _observableArray;
        private ObservableCollection<Int32ArrayItem> _items = new();
        private bool _isUpdating;

        public Int32ArrayEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public Int32ArrayEditor(ObservableValue<int[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Int32 Array";
            set { }
        }

        private void BindToArray(ObservableValue<int[]> array)
        {
            _observableArray = array;
            RebuildItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<int[]>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(int[] values)
        {
            _items.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                ObservableValue<int> observableValue = new ObservableValue<int>(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableValue<int>.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                Int32Editor editor = new Int32Editor(observableValue)
                {
                    Label = $"[{i}]"
                };

                Int32ArrayItem item = new Int32ArrayItem
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

            int[] newArray = new int[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            // newArray[^1] is already 0 (default)

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Length == 0) return;

            int[] newArray = new int[_observableArray.Value.Length - 1];
            Array.Copy(_observableArray.Value, newArray, newArray.Length);

            _observableArray.Value = newArray;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Int32ArrayItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}