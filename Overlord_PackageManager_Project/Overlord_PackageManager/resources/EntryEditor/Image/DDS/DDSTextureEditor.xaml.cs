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

            List<Int32Entry> intEntries = dds.Table.Entries.OfType<Int32Entry>().ToList();

            if (intEntries.Count < 3)
            {
                return;
            }

            List<Int32Entry> lastThree = intEntries.TakeLast(3).ToList();

            uint width = lastThree[0].Value;
            uint height = lastThree[1].Value;
            DDSFormat format = (DDSFormat)lastThree[2].Value;

            BlobEntry? blob = dds.Table.Entries.OfType<BlobEntry>().FirstOrDefault();

            if (blob == null)
            {
                return;
            }

            MetaText.Text = $"Width: {width}   Height: {height}   Format: {format}";

            ImageHost.Content =
                new DDSImageViewer(width, height, format, blob.Value);
        }
    }
}