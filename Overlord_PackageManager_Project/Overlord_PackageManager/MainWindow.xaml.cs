using Microsoft.Win32;
using Overlord_PackageManager.resources;
using Overlord_PackageManager.resources.OMP;
using Overlord_PackageManager.resources.RPK;
using System.IO;
using System.Windows;

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
    }
}