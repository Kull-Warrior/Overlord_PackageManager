using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class BoneScaleEditor : UserControl, IValueEditor
    {
        public BoneScaleEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public BoneScaleEditor(ObservableBoneScale value) : this()
        {
            BindToBoneScale(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableBoneScale boneScale)
            {
                BindToBoneScale(boneScale);
            }
        }

        private void BindToBoneScale(ObservableBoneScale boneScale)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            ScaleXEditor.DataContext = boneScale.ScaleX;
            ScaleYEditor.DataContext = boneScale.ScaleY;
            ScaleZEditor.DataContext = boneScale.ScaleZ;
        }
    }
}