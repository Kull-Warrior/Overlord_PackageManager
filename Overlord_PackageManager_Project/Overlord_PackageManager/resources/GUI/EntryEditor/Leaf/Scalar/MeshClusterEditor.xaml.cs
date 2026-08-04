using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for MeshClusterEditor.xaml
    /// </summary>
    public partial class MeshClusterEditor : UserControl, IValueEditor
    {
        public MeshClusterEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public MeshClusterEditor(ObservableMeshCluster value) : this()
        {
            BindToMeshCluster(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableMeshCluster meshCluster)
            {
                BindToMeshCluster(meshCluster);
            }
        }

        private void BindToMeshCluster(ObservableMeshCluster meshCluster)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            MatrixEditor.DataContext = meshCluster.Matrix;
            HeadEditor.DataContext = meshCluster.Head;
            TailEditor.DataContext = meshCluster.Tail;
            PatchIndexEditor.DataContext = meshCluster.PatchIndex;
            TriangleCountEditor.DataContext = meshCluster.TriangleCount;
        }
    }
}