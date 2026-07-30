using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableMeshBoneShape : INotifyPropertyChanged
    {
        private MeshBoneShape _value;

        // Individual float properties for binding

        public ObservableMatrix3x3 Matrix { get; }

        public ObservableVector3 Head { get; }

        public ObservableVector3 Tail { get; }

        public MeshBoneShape Value => _value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableMeshBoneShape(MeshBoneShape initial)
        {
            _value = initial;

            Matrix = new ObservableMatrix3x3(initial.Matrix);
            Head = new ObservableVector3(initial.Head);
            Tail = new ObservableVector3(initial.Tail);

            // Keep the Matrix4x4 updated when any component changes
            Matrix.PropertyChanged += (s, e) => UpdateDataContext();
            Head.PropertyChanged += (s, e) => UpdateDataContext();
            Tail.PropertyChanged += (s, e) => UpdateDataContext();
        }

        private void UpdateDataContext()
        {
            _value = new MeshBoneShape(
                new Matrix3x3(
                    Matrix.M11, Matrix.M12, Matrix.M13,
                    Matrix.M21, Matrix.M22, Matrix.M23,
                    Matrix.M31, Matrix.M32, Matrix.M33
                ),
                Head.Value,
                Tail.Value
            );

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MeshBoneShape)));
        }
    }
}