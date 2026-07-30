using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class BoneRotationEditor : UserControl, IValueEditor
    {
        public BoneRotationEditor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public BoneRotationEditor(ObservableBoneRotation value) : this()
        {
            BindToBoneRotation(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableBoneRotation boneRotation)
            {
                BindToBoneRotation(boneRotation);
            }
        }

        private void BindToBoneRotation(ObservableBoneRotation boneRotation)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            YawEditor.DataContext = boneRotation.Yaw;
            PitchEditor.DataContext = boneRotation.Pitch;
            RollEditor.DataContext = boneRotation.Roll;
        }
    }
}