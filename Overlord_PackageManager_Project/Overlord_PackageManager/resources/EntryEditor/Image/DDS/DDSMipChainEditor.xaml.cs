using System.Windows.Controls;
using System.Windows;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class DDSMipChainEditor : UserControl
    {
        public DDSMipChainEditor(ListOfDDSTextures list)
        {
            InitializeComponent();

            var ddsEntries = list.Table.Entries
                .OfType<DDSTextures>()
                .ToList();

            int level = 0;

            foreach (var dds in ddsEntries)
            {
                var intEntries = dds.Table.Entries
                    .OfType<Int32Entry>()
                    .ToList();

                if (intEntries.Count < 3)
                    continue;

                var lastThree = intEntries.TakeLast(3).ToList();

                uint width = lastThree[0].varInt;
                uint height = lastThree[1].varInt;
                DDSFormat format = (DDSFormat)lastThree[2].varInt;

                var blob = dds.Table.Entries
                    .OfType<BlobEntry>()
                    .FirstOrDefault();

                if (blob == null)
                    continue;

                var viewer = new DDSImageViewer(width, height, format, blob.varBytes);

                var panel = CreateMipPanel(level, width, height, format, viewer);

                MipContainer.Children.Add(panel);

                level++;
            }
        }

        private UIElement CreateMipPanel(int level, uint width, uint height, DDSFormat format, DDSImageViewer viewer)
        {
            var container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10)
            };

            var label = new TextBlock
            {
                Text = $"Level {level}\n{width}x{height}\n{format}",
                Margin = new Thickness(0, 0, 0, 5),
                FontFamily = new System.Windows.Media.FontFamily("Consolas")
            };

            container.Children.Add(label);
            container.Children.Add(viewer);

            return container;
        }
    }
}