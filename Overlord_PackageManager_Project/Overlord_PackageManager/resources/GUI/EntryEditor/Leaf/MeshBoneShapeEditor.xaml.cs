using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using System.Numerics;
using System.Windows.Controls;

namespace Overlord_PackageManager.resources.GUI.EntryEditor.Leaf
{
    public partial class MeshBoneShapeEditor : UserControl
    {
        private bool _updating;

        public event EventHandler<MeshBoneShape>? ValueChanged;

        public MeshBoneShape Value { get; private set; }

        public MeshBoneShapeEditor()
            : this(new MeshBoneShape(
                new Matrix3x3(
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1),
                Vector3.Zero,
                Vector3.Zero))
        {
        }

        public MeshBoneShapeEditor(MeshBoneShape value)
        {
            InitializeComponent();

            Value = value;

            MatrixEditor.Label = string.Empty;
            HeadEditor.Label = string.Empty;
            TailEditor.Label = string.Empty;

            LoadFromValue();

            MatrixEditor.ValueChanged += MatrixEditor_ValueChanged;
            HeadEditor.ValueChanged += HeadEditor_ValueChanged;
            TailEditor.ValueChanged += TailEditor_ValueChanged;
        }

        public string Label
        {
            get => LabelBlock.Text;
            set => LabelBlock.Text = value;
        }

        public void LoadFromValue()
        {
            _updating = true;

            MatrixEditor.Value = Value.Matrix;
            MatrixEditor.LoadFromValue();

            HeadEditor.Value = Value.Head;
            HeadEditor.LoadFromValue();

            TailEditor.Value = Value.Tail;
            TailEditor.LoadFromValue();

            _updating = false;
        }

        private void MatrixEditor_ValueChanged(object? sender, Matrix3x3 matrix)
        {
            UpdateValue();
        }

        private void HeadEditor_ValueChanged(object? sender, Vector3 vector)
        {
            UpdateValue();
        }

        private void TailEditor_ValueChanged(object? sender, Vector3 vector)
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            if (_updating)
            {
                return;
            }

            Value = new MeshBoneShape(
                MatrixEditor.Value,
                HeadEditor.Value,
                TailEditor.Value);

            ValueChanged?.Invoke(this, Value);
        }
    }
}