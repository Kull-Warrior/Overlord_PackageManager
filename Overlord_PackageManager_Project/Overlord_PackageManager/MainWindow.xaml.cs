using Microsoft.Win32;
using Overlord_PackageManager.resources.GUI;
using Overlord_PackageManager.resources.Data.Files.OMP;
using Overlord_PackageManager.resources.Data.Files.RPK;
using Overlord_PackageManager.resources.Data.EntryTypes.XML;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.DDS;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.GUI.EntryEditor.XML;
using Overlord_PackageManager.resources.Data.EntryTypes.Lua;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Lua;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList;

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

            if (filePath.ToString().ToLower().EndsWith(".omp"))
            {
                mapFile = new OMPFile();
                mapFile.Parse(filePath);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.BuildFileRoot(openFileDialog.SafeFileName, (mapFile.Body.Info.Table, "Map Info Root"), (mapFile.Body.Data.Table, "Map Data Root")));
                RefTableTreeBuilder.AttachDeleteKeyHandler(treeView);
            }
            else
            {
                resourceFile = new ResourcePackFile();
                resourceFile.Read(filePath);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.BuildFileRoot(openFileDialog.SafeFileName, (resourceFile.Body.Data.Table, "Resourcepack Data Root")));
                RefTableTreeBuilder.AttachDeleteKeyHandler(treeView);
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
                case UInt32Entry int32Entry:
                    EditorHost.Content = new UInt32EntryEditor(int32Entry);
                    break;
                case UInt64Entry int64Entry:
                    EditorHost.Content = new UInt64EntryEditor(int64Entry);
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
                case XMLEntry xmlEntry:
                    EditorHost.Content = new XMLAssetEditor(xmlEntry);
                    break;
                case StringListEntry stringListEntry:
                    EditorHost.Content = new StringListEntryEditor(stringListEntry);
                    break;
                case LuaEntry luaEntry:
                    EditorHost.Content = new LuaEntryEditor(luaEntry);
                    break;
                case FloatArrayEntry floatArrayEntry:
                    EditorHost.Content = new FloatArrayEntryEditor(floatArrayEntry);
                    break;
                case UInt16ArrayEntry uint16ArrayEntry:
                    EditorHost.Content = new UInt16ArrayEntryEditor(uint16ArrayEntry);
                    break;
                case MeshData meshData:
                    EditorHost.Content = new MeshDataEditor(meshData);
                    break;
                case VertexDeclarationEntry vertexDeclaration:
                    EditorHost.Content = new VertexDeclarationEditor(vertexDeclaration);
                    break;
                case MatricesArrayEntry matricesArrayEntry:
                    EditorHost.Content = new MatricesArrayEntryEditor(matricesArrayEntry);
                    break;
                case MeshBoneShapeArrayEntry meshBoneShapeArray:
                    EditorHost.Content = new MeshBoneShapeArrayEntryEditor(meshBoneShapeArray);
                    break;
                case RawMeshClusterDataArrayEntry rawMeshClusterDataArray:
                    EditorHost.Content = new RawMeshClusterDataArrayEntryEditor(rawMeshClusterDataArray);
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
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("No file loaded.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = Path.GetFileName(filePath);
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(filePath);

            saveFileDialog.Filter =
                "RPK Files (*.rpk)|*.rpk|" +
                "PRP Files (*.prp)|*.prp|" +
                "PSP Files (*.psp)|*.psp|" +
                "PVP Files (*.pvp)|*.pvp|" +
                "OMP Files (*.omp)|*.omp|" +
                "All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            string savePath = saveFileDialog.FileName;

            try
            {
                if (savePath.EndsWith(".omp", StringComparison.OrdinalIgnoreCase))
                {
                    if (mapFile == null)
                    {
                        MessageBox.Show("No OMP file loaded.");
                        return;
                    }

                    mapFile.Write(savePath);
                }
                else
                {
                    if (resourceFile == null)
                    {
                        MessageBox.Show("No resource file loaded.");
                        return;
                    }

                    resourceFile.Write(savePath);
                }

                MessageBox.Show("File saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}