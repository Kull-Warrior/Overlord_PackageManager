using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for RawMeshClusterDataEditor.xaml
    /// </summary>
    public partial class RawMeshClusterDataEditor : UserControl, IValueEditor
    {
        public RawMeshClusterDataEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public RawMeshClusterDataEditor(ObservableRawMeshClusterData value) : this()
        {
            BindToRawMeshClusterData(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableRawMeshClusterData rawMeshClusterData)
            {
                BindToRawMeshClusterData(rawMeshClusterData);
            }
        }

        private void BindToRawMeshClusterData(ObservableRawMeshClusterData rawMeshClusterData)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            MatrixEditor.DataContext = rawMeshClusterData.Matrix;
            HeadEditor.DataContext = rawMeshClusterData.Head;
            TailEditor.DataContext = rawMeshClusterData.Tail;
            PatchIndexEditor.DataContext = rawMeshClusterData.PatchIndex;
            TriangleCountEditor.DataContext = rawMeshClusterData.TriangleCount;
        }
    }
}