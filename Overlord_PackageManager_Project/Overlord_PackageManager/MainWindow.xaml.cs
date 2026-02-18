using Overlord_PackageManager.resources;
using Overlord_PackageManager.resources.OMP;
using Overlord_PackageManager.resources.RPK;
using System.Windows;

namespace Overlord_PackageManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            OMPFile mapFile = new OMPFile();
            //mapFile.Parse("D:\\Downloads\\26092025\\OL\\OMP\\Tower.omp");

            string baseDir = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Overlord\\Resources\\";
            string fileName = "System Content1";
            string fileExtension = ".prp";
            ResourcePackFile file = new ResourcePackFile();
            file.Read(baseDir + fileName + fileExtension);
            file.WriteAllAssetsToFile(baseDir + fileName);
            treeView.Items.Clear();
            treeView.Items.Add(RefTableTreeBuilder.Build(file.Body.Data, "Root"));
            Console.WriteLine();
        }
    }
}