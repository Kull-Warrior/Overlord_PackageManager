using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawList
{
    /// <summary>
    /// View model for a single uint item in the list
    /// </summary>
    public class VertexAttributeListItem
    {
        public VertexAttributeEditor? Editor { get; set; }
        public ObservableVertexAttribute ObservableValue { get; set; } = null!;
    }

    /// <summary>
    /// Interaction logic for VertexAttributeListEditor.xaml
    /// </summary>
    public partial class VertexAttributeListEditor : UserControl, IValueEditor
    {
        private ObservableValue<List<VertexAttribute>>? _observableArray;
        private ObservableCollection<VertexAttributeListItem> _items = new();
        private bool _isUpdating;

        public VertexAttributeListEditor()
        {
            InitializeComponent();
            ArrayItemsControl.ItemsSource = _items;
        }

        public VertexAttributeListEditor(ObservableValue<List<VertexAttribute>> list) : this()
        {
            BindToArray(list);
        }

        public string Label
        {
            get => "VertexAttribute List";
            set { }
        }

        private void BindToArray(ObservableValue<List<VertexAttribute>> list)
        {
            _observableArray = list;
            RebuildItems(list.Value);
            list.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<List<VertexAttribute>>.Value))
            {
                _isUpdating = true;
                RebuildItems(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void RebuildItems(List<VertexAttribute> values)
        {
            _items.Clear();

            for (int i = 0; i < values.Count; i++)
            {
                ObservableVertexAttribute observableValue = new ObservableVertexAttribute(values[i]);

                // When this individual value changes, update the full array
                observableValue.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ObservableVertexAttribute.Value))
                    {
                        UpdateArrayFromItems();
                    }
                };

                VertexAttributeEditor editor = new VertexAttributeEditor(observableValue)
                {
                    Label = $"[{i}]"
                };

                VertexAttributeListItem item = new VertexAttributeListItem
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
            _observableArray.Value = _items.Select(item => item.ObservableValue.Value).ToList();
            _isUpdating = false;
        }

        private void UpdateCountDisplay()
        {
            CountBlock.Text = $"Count: {_items.Count}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            List<VertexAttribute> newList = new List<VertexAttribute>(_observableArray.Value.Count + 1);
            newList.AddRange(_observableArray.Value);
            // newList[^1] is already 0 (default)

            _observableArray.Value = newList;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null || _observableArray.Value.Count == 0) return;

            List<VertexAttribute> newList = new List<VertexAttribute>(_observableArray.Value.Count - 1);
            newList.AddRange(_observableArray.Value.Take(_observableArray.Value.Count - 1));

            _observableArray.Value = newList;
        }

        private void RemoveSpecificButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is VertexAttributeListItem item)
            {
                _items.Remove(item);
                UpdateArrayFromItems();
                UpdateCountDisplay();
            }
        }
    }
}