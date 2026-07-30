using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for TransformEditor.xaml
    /// </summary>
    public partial class TransformEditor : UserControl, IValueEditor
    {
        public TransformEditor()
        {
            InitializeComponent();
        }

        public TransformEditor(ObservableTransform value) : this()
        {
            BindToDataContext(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void BindToDataContext(ObservableTransform transform)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            MatrixEditor.DataContext = transform.Matrix;
            RotationEditor.DataContext = transform.Rotation;
            TranslationEditor.DataContext = transform.Translation;
        }
    }
}