using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Matrix3x3Editor.xaml
    /// </summary>
    public partial class Matrix3x3Editor : UserControl, IValueEditor
    {
        public Matrix3x3Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public Matrix3x3Editor(ObservableMatrix3x3 value) : this()
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
            if (e.NewValue is ObservableMatrix3x3 matrix)
            {
                BindToMatrix(matrix);
            }
        }

        private void BindToMatrix(ObservableMatrix3x3 matrix)
        {
            M11Editor.DataContext = matrix.M11;
            M12Editor.DataContext = matrix.M12;
            M13Editor.DataContext = matrix.M13;
            M21Editor.DataContext = matrix.M21;
            M22Editor.DataContext = matrix.M22;
            M23Editor.DataContext = matrix.M23;
            M31Editor.DataContext = matrix.M31;
            M32Editor.DataContext = matrix.M32;
            M33Editor.DataContext = matrix.M33;
        }
    }
}