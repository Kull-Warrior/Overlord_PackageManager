using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class Vector4Editor : UserControl, IValueEditor
    {
        public Vector4Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public Vector4Editor(ObservableVector4 value) : this()
        {
            BindToVector4(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableVector4 vector)
            {
                BindToVector4(vector);
            }
        }

        private void BindToVector4(ObservableVector4 vector)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            XEditor.DataContext = vector.X;
            YEditor.DataContext = vector.Y;
            ZEditor.DataContext = vector.Z;
            WEditor.DataContext = vector.W;
        }
    }
}