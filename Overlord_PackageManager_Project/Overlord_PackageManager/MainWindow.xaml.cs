using Microsoft.Win32;
using Overlord_PackageManager.resources;
using Overlord_PackageManager.resources.EntryEditor;
using Overlord_PackageManager.resources.EntryTypes.BaseTypes;
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
            if (openFileDialog.ShowDialog() == true)
            {
                filePath.Text = openFileDialog.FileName;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath.Text) || !File.Exists(filePath.Text))
            {
                MessageBox.Show("Please select a valid resource file.");
                return;
            }

            if (filePath.Text.EndsWith(".omp"))
            {
                mapFile = new OMPFile();
                mapFile.Parse(filePath.Text);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.Build(mapFile.Body.Data, "Root"));
            }
            else
            {
                resourceFile = new ResourcePackFile();
                resourceFile.Read(filePath.Text);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.Build(resourceFile.Body.Data, "Root"));
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath.Text) || !File.Exists(filePath.Text))
            {
                MessageBox.Show("Please select a valid resource file.");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(filePath.Text);
            string parentDir = dirInfo.Parent.FullName + "\\";
            string dirName = Path.GetFileNameWithoutExtension(filePath.Text);

            if (filePath.Text.EndsWith(".omp"))
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
                default:
                    EditorHost.Content = new TextBlock
                    {
                        Text = obj?.ToString() ?? "null"
                    };
                    break;
            }
        }
    }
}