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

            RpkFile file = new RpkFile();
            file.Parse("D:\\Downloads\\26092025\\OL\\RPK FIles\\System.psp");

            Console.WriteLine();
        }
    }
}