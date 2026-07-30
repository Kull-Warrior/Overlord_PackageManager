using Microsoft.Win32;
using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray
{
    public class HexLine
    {
        public int Offset { get; }
        public string Hex { get; }
        public string Ascii { get; }

        public HexLine(int offset, string hex, string ascii)
        {
            Offset = offset;
            Hex = hex;
            Ascii = ascii;
        }
    }

    public static class HexFormatter
    {
        public static IEnumerable<HexLine> Format(byte[] data, int bytesPerLine = 16)
        {
            for (int i = 0; i < data.Length; i += bytesPerLine)
            {
                var slice = data.Skip(i).Take(bytesPerLine).ToArray();

                var hex = string.Join(" ", slice.Select(b => b.ToString("X2")));

                var ascii = new string(slice.Select(b =>
                    b >= 32 && b <= 126 ? (char)b : '.'
                ).ToArray());

                yield return new HexLine(i, hex, ascii);
            }
        }
    }

    public partial class ByteArrayEntryEditor : UserControl, IValueEditor
    {
        private ObservableValue<byte[]>? _observableArray;
        private bool _isUpdating;

        public ByteArrayEntryEditor()
        {
            InitializeComponent();
        }

        public ByteArrayEntryEditor(ObservableValue<byte[]> array) : this()
        {
            BindToArray(array);
        }

        public string Label
        {
            get => "Byte Array"; // Could add a Label control if needed
            set { } // Implement if you add a Label control
        }

        private void BindToArray(ObservableValue<byte[]> array)
        {
            _observableArray = array;

            // Display initial data
            HexList.ItemsSource = HexFormatter.Format(array.Value);

            // Listen for external changes
            array.PropertyChanged += OnArrayChanged;
        }

        private void OnArrayChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!_isUpdating && e.PropertyName == nameof(ObservableValue<byte[]>.Value))
            {
                _isUpdating = true;
                HexList.ItemsSource = HexFormatter.Format(_observableArray!.Value);
                _isUpdating = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            var dialog = new SaveFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin",
                DefaultExt = ".bin",
                FileName = "export.bin"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllBytes(dialog.FileName, _observableArray.Value);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (_observableArray == null) return;

            var dialog = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin"
            };

            if (dialog.ShowDialog() == true)
            {
                byte[] newData = File.ReadAllBytes(dialog.FileName);

                // This will trigger PropertyChanged and update the display
                _observableArray.Value = newData;
            }
        }
    }
}