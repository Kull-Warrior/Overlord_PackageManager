using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.ComponentModel;
using System.Windows;
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

            DataContextChanged += OnDataContextChanged;
            StringBox.TextChanged += OnTextChanged;

            UpdateCharCount();
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

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            nameof(MaxLength),
            typeof(int),
            typeof(StringEditor),
            new PropertyMetadata(0)
        );

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ObservableValue<char[]> oldArray)
            {
                oldArray.PropertyChanged -= OnArrayChanged;
            }

            if (e.NewValue is ObservableValue<char[]> newArray)
            {
                BindToArray(newArray);
            }
            else
            {
                _observableArray = null;
            }
        }

        private void BindToArray(ObservableValue<char[]> array)
        {
            if (_observableArray != null)
            {
                _observableArray.PropertyChanged -= OnArrayChanged;
            }

            _observableArray = array;
            _observableArray.PropertyChanged += OnArrayChanged;

            SetTextFromArray(array.Value);
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObservableValue<char[]>.Value))
                return;

            if (_observableArray == null)
                return;

            SetTextFromArray(_observableArray.Value);
        }

        private void OnTextChanged(object sender,TextChangedEventArgs e)
        {
            if (_isUpdating || _observableArray == null)
                return;

            _observableArray.Value = StringBox.Text.ToCharArray();

            UpdateCharCount();
        }

        private void SetTextFromArray(char[] value)
        {
            string text = new string(value ?? Array.Empty<char>());

            if (StringBox.Text == text)
            {
                UpdateCharCount();
                return;
            }

            _isUpdating = true;

            try
            {
                StringBox.Text = text;
                StringBox.CaretIndex = StringBox.Text.Length;
            }
            finally
            {
                _isUpdating = false;
            }

            UpdateCharCount();
        }

        private void UpdateCharCount()
        {
            CountBlock.Text = $"Length: {StringBox.Text.Length} characters";
        }
    }
}