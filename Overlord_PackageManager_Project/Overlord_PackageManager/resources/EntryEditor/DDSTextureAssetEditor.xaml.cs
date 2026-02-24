using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.EntryEditor
{
    public partial class DDSTextureAssetEditor : UserControl
    {
        public DDSTextureAssetEditor(DDSTextureAsset asset)
        {
            InitializeComponent();

            BuildUI(asset);
        }

        private void BuildUI(DDSTextureAsset asset)
        {
            // Assuming asset exposes a ReferenceTable like others
            List<Generic.Entry> entries = asset.Table.Entries;

            List<StringEntry> stringEntries = entries
                .OfType<StringEntry>()
                .ToList();

            DDSTextureAssetDataContainer? mipContainer = entries
                .OfType<DDSTextureAssetDataContainer>()
                .FirstOrDefault();

            if (stringEntries.Count >= 2)
            {
                StringEntryEditor tagEditor = new StringEntryEditor(stringEntries[0]);
                StringEntryEditor fileNameEditor = new StringEntryEditor(stringEntries[1]);

                RootPanel.Children.Add(CreateSection("Tag", tagEditor));
                RootPanel.Children.Add(CreateSection("File Name", fileNameEditor));
            }

            if (mipContainer != null)
            {
                ListOfDDSTextures? mipList = mipContainer.Table.Entries
                    .OfType<ListOfDDSTextures>()
                    .FirstOrDefault();

                if (mipList != null)
                {
                    DDSMipChainEditor mipEditor = new DDSMipChainEditor(mipList);
                    RootPanel.Children.Add(CreateSection("Mip Chain", mipEditor));
                }
            }
        }

        private UIElement CreateSection(string title, UIElement content)
        {
            StackPanel panel = new StackPanel
            {
                Margin = new Thickness(0, 0, 0, 15)
            };

            panel.Children.Add(new TextBlock
            {
                Text = title,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            });

            panel.Children.Add(content);

            return panel;
        }
    }
}