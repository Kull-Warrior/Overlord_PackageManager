using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    /// <summary>
    /// Observable wrapper for MeshBoneShape.
    /// </summary>
    public class ObservableMeshBoneShape : ObservableComposite
    {
        private MeshBoneShape _value;

        public ObservableMatrix3x3 Matrix { get; }
        public ObservableVector3 Head { get; }
        public ObservableVector3 Tail { get; }

        public MeshBoneShape Value => _value;

        public ObservableMeshBoneShape(MeshBoneShape initial)
        {
            _value = initial;

            Matrix = new ObservableMatrix3x3(initial.Matrix);
            Head = new ObservableVector3(initial.Head);
            Tail = new ObservableVector3(initial.Tail);

            Subscribe(Matrix, Head, Tail);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            // We only care when one of the child Value properties changes.
            if (e.PropertyName != nameof(Value))
                return;

            _value = new MeshBoneShape(Matrix.Value, Head.Value, Tail.Value);

            OnPropertyChanged(nameof(Value));
        }
    }
}