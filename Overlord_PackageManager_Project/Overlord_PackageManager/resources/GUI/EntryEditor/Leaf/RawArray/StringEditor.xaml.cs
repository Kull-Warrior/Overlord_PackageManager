using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.ComponentModel;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    /// <summary>
    /// Interaction logic for StringEditor.xaml
    /// </summary>
    public partial class StringEditor : UserControl, IValueEditor
    {
        private ObservableValue<char[]>? _observableArray;
        private bool _isUpdating;

        public StringEditor()
        {
            InitializeComponent();
            StringBox.TextChanged += OnTextChanged;
        }

        public StringEditor(ObservableValue<char[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => HeaderLabel.Text;
            set => HeaderLabel.Text = value;
        }

        private void BindToArray(ObservableValue<char[]> array)
        {
            _observableArray = array;

            // Set initial text
            StringBox.Text = new string(array.Value);
            UpdateCharCount();

            // Listen for external changes
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<char[]>.Value))
            {
                _isUpdating = true;
                StringBox.Text = new string(_observableArray!.Value);
                UpdateCharCount();
                _isUpdating = false;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isUpdating && _observableArray != null)
            {
                _isUpdating = true;
                _observableArray.Value = StringBox.Text.ToCharArray();
                UpdateCharCount();
                _isUpdating = false;
            }
        }

        private void UpdateCharCount()
        {
            CountBlock.Text = $"Length: {StringBox.Text.Length} characters";
        }
    }
}