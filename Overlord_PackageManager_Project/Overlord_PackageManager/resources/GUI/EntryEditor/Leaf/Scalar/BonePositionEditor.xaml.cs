using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class BonePositionEditor : UserControl, IValueEditor
    {
        public BonePositionEditor()
        {
            InitializeComponent();
        }

        public BonePositionEditor(ObservableBonePosition value) : this()
        {
            BindToBonePosition(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void BindToBonePosition(ObservableBonePosition bonePosition)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            TimestampEditor.DataContext = bonePosition.Timestamp;
            XEditor.DataContext = bonePosition.X;
            YEditor.DataContext = bonePosition.Y;
            ZEditor.DataContext = bonePosition.Z;
        }
    }
}