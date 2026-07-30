using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
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
            DataContextChanged += OnDataContextChanged;
        }

        public ObjectBoneEditor(ObservableObjectBone value) : this()
        {
            BindToObjectBone(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableObjectBone objectBone)
            {
                BindToObjectBone(objectBone);
            }
        }

        private void BindToObjectBone(ObservableObjectBone objectBone)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            TransformEditor.DataContext = objectBone.Transform;
        }
    }
}