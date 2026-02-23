using System.Windows.Controls;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class DDSTextureEditor : UserControl
    {
        public DDSTextureEditor(DDSTextures dds)
        {
            InitializeComponent();

            var intEntries = dds.Table.Entries
                .OfType<Int32Entry>()
                .ToList();

            if (intEntries.Count < 3)
                return;

            var lastThree = intEntries.TakeLast(3).ToList();

            uint width = lastThree[0].varInt;
            uint height = lastThree[1].varInt;
            DDSFormat format = (DDSFormat)lastThree[2].varInt;

            var blob = dds.Table.Entries
                .OfType<BlobEntry>()
                .FirstOrDefault();

            if (blob == null)
                return;

            MetaText.Text = $"Width: {width}   Height: {height}   Format: {format}";

            ImageHost.Content =
                new DDSImageViewer(width, height, format, blob.varBytes);
        }
    }
}