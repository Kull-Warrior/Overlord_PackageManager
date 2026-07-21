using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.GUI.Leaf;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class RawMeshClusterDataEditor : UserControl
    {
        private RawMeshClusterData _value;

        public event EventHandler<RawMeshClusterData>? ValueChanged;

        public string Label
        {
            get => RootGroup.Header?.ToString() ?? string.Empty;
            set => RootGroup.Header = value;
        }

        public RawMeshClusterDataEditor(RawMeshClusterData value)
        {
            InitializeComponent();

            _value = value;

            Matrix3x3Editor matrixEditor = new(value.Matrix)
            {
                Label = "Matrix"
            };

            Vector3Editor centerEditor = new(value.Center)
            {
                Label = "Center"
            };

            Vector3Editor extentsEditor = new(value.Extents)
            {
                Label = "Extents"
            };

            UInt16Editor patchIndexEditor = new(value.patchIndex)
            {
                Label = "Patch Index"
            };

            UInt16Editor triangleCountEditor = new(value.triangleCount)
            {
                Label = "Triangle Count"
            };

            matrixEditor.ValueChanged += (_, v) =>
            {
                _value = _value with { Matrix = v };
                NotifyChanged();
            };

            centerEditor.ValueChanged += (_, v) =>
            {
                _value = _value with { Center = v };
                NotifyChanged();
            };

            extentsEditor.ValueChanged += (_, v) =>
            {
                _value = _value with { Extents = v };
                NotifyChanged();
            };

            patchIndexEditor.ValueChanged += (_, v) =>
            {
                _value = _value with { patchIndex = v };
                NotifyChanged();
            };

            triangleCountEditor.ValueChanged += (_, v) =>
            {
                _value = _value with { triangleCount = v };
                NotifyChanged();
            };

            MatrixHost.Content = matrixEditor;
            CenterHost.Content = centerEditor;
            ExtentsHost.Content = extentsEditor;
            PatchIndexHost.Content = patchIndexEditor;
            TriangleCountHost.Content = triangleCountEditor;
        }

        private void NotifyChanged()
        {
            ValueChanged?.Invoke(this, _value);
        }
    }
}