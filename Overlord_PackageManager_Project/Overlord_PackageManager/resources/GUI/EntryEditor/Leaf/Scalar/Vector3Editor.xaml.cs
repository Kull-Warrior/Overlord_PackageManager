using Overlord_PackageManager.resources.GUI.Interfaces;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar
{
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class Vector3Editor : UserControl, IValueEditor
    {
        public Vector3Editor()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public Vector3Editor(ObservableVector3 value) : this()
        {
            BindToVector3(value);
        }

        public string Label
        {
            get => MainLabel.Content?.ToString() ?? string.Empty;
            set => MainLabel.Content = value;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ObservableVector3 vector)
            {
                BindToVector3(vector);
            }
        }

        private void BindToVector3(ObservableVector3 vector)
        {
            // Bind each FloatEditor to its corresponding ObservableValue<float>
            XEditor.DataContext = vector.X;
            YEditor.DataContext = vector.Y;
            ZEditor.DataContext = vector.Z;
        }
    }
}