using Microsoft.Win32;
using Overlord_PackageManager.resources;
using Overlord_PackageManager.resources.EntryEditor;
using Overlord_PackageManager.resources.EntryTypes.Audio;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
using Overlord_PackageManager.resources.EntryTypes.Image.DDS;
using Overlord_PackageManager.resources.OMP;
using Overlord_PackageManager.resources.RPK;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ResourcePackFile resourceFile;
        OMPFile mapFile;

        string filePath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "RPK Files (*.rpk)|*.rpk|" +
                "PRP Files (*.prp)|*.prp|" +
                "PSP Files (*.psp)|*.psp|" +
                "PVP Files (*.pvp)|*.pvp|" +
                "OMP Files (*.omp)|*.omp|" +
                "All files (*.*)| *.*";
            openFileDialog.FilterIndex = 6;
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid resource file.");
                return;
            }

            if (filePath.EndsWith(".omp"))
            {
                mapFile = new OMPFile();
                mapFile.Parse(filePath);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.Build(mapFile.Body.Data, "Root"));
            }
            else
            {
                resourceFile = new ResourcePackFile();
                resourceFile.Read(filePath);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.Build(resourceFile.Body.Data, "Root"));
            }
        }

        private void ExportAssets(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid resource file.");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            string parentDir = dirInfo.Parent.FullName + "\\";
            string dirName = Path.GetFileNameWithoutExtension(filePath);

            if (filePath.EndsWith(".omp"))
            {
                //mapFile.WriteAllAssetsToFile(parentDir + dirName);
            }
            else
            {
                resourceFile.WriteAllAssetsToFile(parentDir + dirName);
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is TreeViewItem item)
            {
                ShowEditor(item.Tag);
            }
        }

        private void ShowEditor(object obj)
        {
            EditorHost.Content = null;

            switch (obj)
            {
                case Int32Entry int32Entry:
                    EditorHost.Content = new Int32EntryEditor(int32Entry);
                    break;
                case Int64Entry int64Entry:
                    EditorHost.Content = new Int64EntryEditor(int64Entry);
                    break;
                case FloatEntry floatEntry:
                    EditorHost.Content = new FloatEntryEditor(floatEntry);
                    break;
                case BlobEntry blobEntry:
                    EditorHost.Content = new BlobEntryEditor(blobEntry);
                    break;
                case StringEntry stringEntry:
                    EditorHost.Content = new StringEntryEditor(stringEntry);
                    break;
                case DDSTextures ddsTextures:
                    EditorHost.Content = new DDSTextureEditor(ddsTextures);
                    break;
                case DDSTextureAsset asset:
                    EditorHost.Content = new DDSTextureAssetEditor(asset);
                    break;
                case ReflectionCubeMapAsset reflectionCubeMap:
                    EditorHost.Content = new ReflectionCubeMapEditor(reflectionCubeMap);
                    break;
                case SFXAsset sfxAsset:
                    EditorHost.Content = new SFXAssetEditor(sfxAsset);
                    break;
                default:
                    EditorHost.Content = new TextBlock
                    {
                        Text = obj?.ToString() ?? "null"
                    };
                    break;
            }
        }

        private void New(object sender, RoutedEventArgs e)
        {

        }

        private void Save(object sender, RoutedEventArgs e)
        {

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}