using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.EntryEditor
{
    /// <summary>
    /// Interaktionslogik für BlobEntryEditor.xaml
    /// </summary>

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

    public partial class BlobEntryEditor : UserControl
    {
        private readonly BlobEntry _entry;

        public BlobEntryEditor(BlobEntry entry)
        {
            InitializeComponent();
            _entry = entry;

            HexList.ItemsSource = HexFormatter.Format(entry.varBytes);
        }
    }
}
