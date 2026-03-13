using System.Windows.Controls;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf;
using Overlord_PackageManager.resources.Data.Files.DDS;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.DDS
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