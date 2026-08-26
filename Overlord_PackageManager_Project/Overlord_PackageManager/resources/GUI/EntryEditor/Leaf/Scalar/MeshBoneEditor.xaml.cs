using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for MeshBoneEditor.xaml
    /// </summary>
    public partial class MeshBoneEditor : UserControl, IValueEditor
    {
        public MeshBoneEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public MeshBoneEditor(ObservableMeshBone value) : this()
        {
            BindToMeshBone(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableMeshBone meshBone)
            {
                BindToMeshBone(meshBone);
            }
        }

        private void BindToMeshBone(ObservableMeshBone meshBone)
        {
            NameEditor.DataContext = meshBone.Name;
            TransformEditor.DataContext = meshBone.Transform;
            Unknown1Editor.DataContext = meshBone.Unknown1;
            Unknown2Editor.DataContext = meshBone.Unknown2;
            Unknown3Editor.DataContext = meshBone.Unknown3;
            Unknown4Editor.DataContext = meshBone.Unknown4;
            Unknown5Editor.DataContext = meshBone.Unknown5;
            Unknown6Editor.DataContext = meshBone.Unknown6;
        }
    }
}