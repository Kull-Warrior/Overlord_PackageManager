using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Matrix4x4Editor.xaml
    /// </summary>
    public partial class MeshBoneShapeEditor : UserControl, IValueEditor
    {
        public MeshBoneShapeEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public MeshBoneShapeEditor(ObservableMeshBoneShape value) : this()
        {
            BindToMeshBoneShape(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableMeshBoneShape meshBoneShape)
            {
                BindToMeshBoneShape(meshBoneShape);
            }
        }

        private void BindToMeshBoneShape(ObservableMeshBoneShape meshBoneShape)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            MatrixEditor.DataContext = meshBoneShape.Matrix;
            HeadEditor.DataContext = meshBoneShape.Head;
            TailEditor.DataContext = meshBoneShape.Tail;
        }
    }
}