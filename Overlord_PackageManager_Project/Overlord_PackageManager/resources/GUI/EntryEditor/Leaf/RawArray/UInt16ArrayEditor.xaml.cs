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
    /// View model for a single ushort item in the array
    /// </summary>
    public class UInt16ArrayItem
    {
        public UInt16Editor? Editor { get; set; }
        public ObservableValue<ushort> ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for UInt16ArrayEditor.xaml
    /// </summary>
    public partial class UInt16ArrayEditor : UserControl, IValueEditor
    {
        private ObservableValue<ushort[]>? _observableArray;
        private ObservableCollection<UInt16ArrayItem> _items = new();
        private bool _isUpdating;

        public UInt16ArrayEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public UInt16ArrayEditor(ObservableValue<ushort[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "UInt16 Array";
            set { }
        }

        private void BindToArray(ObservableValue<ushort[]> array)
        {
            _observableArray = array;
            RebuildItems(array.Value);
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<ushort[]>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(ushort[] values)
        {
            _items.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                ObservableValue<ushort> observableValue = new ObservableValue<ushort>(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableValue<ushort>.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                UInt16Editor editor = new UInt16Editor(observableValue)
                {
                    Label = $"[{i}]"
                };

                UInt16ArrayItem item = new UInt16ArrayItem
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

            ushort[] newArray = new ushort[_observableArray.Value.Length + 1];
            Array.Copy(_observableArray.Value, newArray, _observableArray.Value.Length);
            // newArray[^1] is already 0 (default)

            _observableArray.Value = newArray;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Length == 0) return;

            ushort[] newArray = new ushort[_observableArray.Value.Length - 1];
            Array.Copy(_observableArray.Value, newArray, newArray.Length);

            _observableArray.Value = newArray;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is UInt16ArrayItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}