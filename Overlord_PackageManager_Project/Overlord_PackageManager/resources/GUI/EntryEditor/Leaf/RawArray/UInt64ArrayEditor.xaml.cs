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
    /// View model for a single uint item in the array
    /// </summary>
    public class UInt64ArrayItem
    {
        public UInt64Editor? Editor { get; set; }
        public ObservableValue<ulong> ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for UInt64ArrayEditor.xaml
    /// </summary>
    public partial class UInt64ArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<ulong[]>? _observableArray;
        private ObservableCollection<UInt64ArrayItem> _items = new();
        private bool _isUpdating;

        public UInt64ArrayEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public UInt64ArrayEditor(ObservableValue<ulong[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "UInt64 Array";
            set { }
        }

        private void BindToArray(ObservableValue<ulong[]> array)
        {
            _observableArray = array;
            RebuildItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<ulong[]>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(ulong[] values)
        {
            _items.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                ObservableValue<ulong> observableValue = new ObservableValue<ulong>(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableValue<ulong>.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                UInt64Editor editor = new UInt64Editor(observableValue)
                {
                    Label = $"[{i}]"
                };

                UInt64ArrayItem item = new UInt64ArrayItem
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

            ulong[] newArray = new ulong[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            // newArray[^1] is already 0 (default)

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Length == 0) return;

            ulong[] newArray = new ulong[_observableArray.Value.Length - 1];
            Array.Copy(_observableArray.Value, newArray, newArray.Length);

            _observableArray.Value = newArray;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is UInt64ArrayItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}