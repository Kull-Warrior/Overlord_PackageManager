using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class QuaternionEditor : UserControl, IValueEditor
    {
        public QuaternionEditor()
        {
            InitializeComponent();
        }

        public QuaternionEditor(ObservableQuaternion value) : this()
        {
            BindToQuaternion(value);

            XEditor.ValueTextBox.IsReadOnly = true;
            YEditor.ValueTextBox.IsReadOnly = true;
            ZEditor.ValueTextBox.IsReadOnly = true;
            WEditor.ValueTextBox.IsReadOnly = true;
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void BindToQuaternion(ObservableQuaternion quaternion)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            XEditor.DataContext = quaternion.X;
            YEditor.DataContext = quaternion.Y;
            ZEditor.DataContext = quaternion.Z;
            WEditor.DataContext = quaternion.W;
        }
    }
}