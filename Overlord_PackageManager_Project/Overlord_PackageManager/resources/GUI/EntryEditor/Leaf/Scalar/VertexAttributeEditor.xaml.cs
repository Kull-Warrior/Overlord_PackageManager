using System.Windows;
using System.Windows.Controls;
using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for VertexAttributeEditor.xaml
    /// </summary>
    public partial class VertexAttributeEditor : UserControl, IValueEditor
    {
        private ObservableVertexAttribute? _observableData;
        private bool _isUpdating;

        public VertexAttributeEditor()
        {
            InitializeComponent();

            // Setup dropdown items
            SemanticBox.ItemsSource = Enum.GetValues(typeof(VertexAttributeSemantic)).Cast<VertexAttributeSemantic>();
            SizeBox.ItemsSource = ObservableVertexAttribute.AllowedSizes;

            DataContextChanged += OnDataContextChanged;
        }

        public VertexAttributeEditor(ObservableVertexAttribute value) : this()
        {
            BindToData(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableVertexAttribute data)
            {
                BindToData(data);
            }
        }

        private void BindToData(ObservableVertexAttribute data)
        {
            _observableData = data;

            // Setup bindings
            SetupBindings();

            // Initial display update
            UpdateRawDisplay();

            // Listen for changes from external sources
            data.PropertyChanged += OnExternalDataChanged;
        }

        private void SetupBindings()
        {
            if (_observableData == null) return;

            // Type binding
            TypeBox.Text = _observableData.Type.Value.ToString();
            TypeBox.TextChanged += (s, e) =>
            {
                if (_isUpdating) return;
                if (byte.TryParse(TypeBox.Text, out byte value))
                {
                    _isUpdating = true;
                    _observableData.Type.Value = value;
                    UpdateRawDisplay();
                    _isUpdating = false;
                }
            };

            // Index binding
            IndexBox.Text = _observableData.Index.Value.ToString();
            IndexBox.TextChanged += (s, e) =>
            {
                if (_isUpdating) return;
                if (byte.TryParse(IndexBox.Text, out byte value))
                {
                    _isUpdating = true;
                    _observableData.Index.Value = value;
                    UpdateRawDisplay();
                    _isUpdating = false;
                }
            };

            // Semantic binding
            SemanticBox.SelectedItem = _observableData.Semantic.Value;
            SemanticBox.SelectionChanged += (s, e) =>
            {
                if (_isUpdating) return;
                if (SemanticBox.SelectedItem is VertexAttributeSemantic semantic)
                {
                    _isUpdating = true;
                    _observableData.Semantic.Value = semantic;
                    UpdateRawDisplay();
                    _isUpdating = false;
                }
            };

            // ByteSize binding
            SizeBox.SelectedItem = _observableData.ByteSize.Value;
            SizeBox.SelectionChanged += (s, e) =>
            {
                if (_isUpdating) return;
                if (SizeBox.SelectedItem is byte size)
                {
                    _isUpdating = true;
                    _observableData.ByteSize.Value = size;
                    UpdateRawDisplay();
                    _isUpdating = false;
                }
            };

            // Subscribe to individual component changes
            _observableData.Type.PropertyChanged += (s, e) => RefreshDisplay();
            _observableData.Index.PropertyChanged += (s, e) => RefreshDisplay();
            _observableData.Semantic.PropertyChanged += (s, e) => RefreshDisplay();
            _observableData.ByteSize.PropertyChanged += (s, e) => RefreshDisplay();
        }

        private void OnExternalDataChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ObservableVertexAttribute.Value))
            {
                RefreshDisplay();
            }
        }

        private void RefreshDisplay()
        {
            if (_isUpdating || _observableData == null) return;

            _isUpdating = true;
            TypeBox.Text = _observableData.Type.Value.ToString();
            IndexBox.Text = _observableData.Index.Value.ToString();

            if (SemanticBox.SelectedItem is not VertexAttributeSemantic currentSemantic || currentSemantic != _observableData.Semantic.Value)
            {
                SemanticBox.SelectedItem = _observableData.Semantic.Value;
            }

            if (SizeBox.SelectedItem is not byte currentSize || currentSize != _observableData.ByteSize.Value)
            {
                SizeBox.SelectedItem = _observableData.ByteSize.Value;
            }

            UpdateRawDisplay();
            _isUpdating = false;
        }

        private void UpdateRawDisplay()
        {
            if (_observableData != null)
            {
                RawBlock.Text = $"Raw: 0x{_observableData.Value.RawDescriptor:X8}";
            }
        }
    }
}