using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for MeshTransformEditor.xaml
    /// </summary>
    public partial class MeshTransformEditor : UserControl, IValueEditor
    {
        public MeshTransformEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public MeshTransformEditor(ObservableMeshTransform value) : this()
        {
            BindToTransform(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableMeshTransform transform)
            {
                BindToTransform(transform);
            }
        }

        private void BindToTransform(ObservableMeshTransform transform)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            MatrixEditor.DataContext = transform.Matrix;
            ScaleEditor.DataContext = transform.Scale;
            TranslationEditor.DataContext = transform.Translation;
            RotationEditor.DataContext = transform.Rotation;
        }
    }
}