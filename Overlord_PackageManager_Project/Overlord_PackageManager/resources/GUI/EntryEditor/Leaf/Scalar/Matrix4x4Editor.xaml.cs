using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Matrix4x4Editor.xaml
    /// </summary>
    public partial class Matrix4x4Editor : UserControl, IValueEditor
    {
        public Matrix4x4Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public Matrix4x4Editor(ObservableMatrix4x4 value) : this()
        {
            BindToMatrix(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableMatrix4x4 matrix)
            {
                BindToMatrix(matrix);
            }
        }

        private void BindToMatrix(ObservableMatrix4x4 matrix)
        {
            M11Editor.DataContext = matrix.M11;
            M12Editor.DataContext = matrix.M12;
            M13Editor.DataContext = matrix.M13;
            M14Editor.DataContext = matrix.M14;

            M21Editor.DataContext = matrix.M21;
            M22Editor.DataContext = matrix.M22;
            M23Editor.DataContext = matrix.M23;
            M24Editor.DataContext = matrix.M24;

            M31Editor.DataContext = matrix.M31;
            M32Editor.DataContext = matrix.M32;
            M33Editor.DataContext = matrix.M33;
            M34Editor.DataContext = matrix.M34;

            M41Editor.DataContext = matrix.M41;
            M42Editor.DataContext = matrix.M42;
            M43Editor.DataContext = matrix.M43;
            M44Editor.DataContext = matrix.M44;
        }
    }
}