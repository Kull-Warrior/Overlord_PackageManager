using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for ObjectBoneEditor.xaml
    /// </summary>
    public partial class ObjectBoneEditor : UserControl, IValueEditor
    {
        public ObjectBoneEditor()
        {
            InitializeComponent();
        }

        public ObjectBoneEditor(ObservableObjectBone value) : this()
        {
            BindToDataContext(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void BindToDataContext(ObservableObjectBone objectBone)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            TransformEditor.DataContext = objectBone.Transform;
        }
    }
}