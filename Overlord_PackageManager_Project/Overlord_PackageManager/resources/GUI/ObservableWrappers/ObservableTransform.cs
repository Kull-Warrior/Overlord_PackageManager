using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableTransform : INotifyPropertyChanged
    {
        private Transform _value;

        // Individual float properties for binding

        public ObservableMatrix4x4 Matrix { get; }

        public ObservableQuaternion Rotation { get; }

        public ObservableVector4 Translation { get; }

        public Transform Value => _value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableTransform(Transform initial)
        {
            _value = initial;

            Matrix = new ObservableMatrix4x4(initial.Matrix);
            Rotation = new ObservableQuaternion(initial.Rotation);
            Translation = new ObservableVector4(initial.Translation);

            // Keep the Transform updated when any component changes
            Matrix.PropertyChanged += (s, e) => UpdateDataContext();
            Rotation.PropertyChanged += (s, e) => UpdateDataContext();
            Translation.PropertyChanged += (s, e) => UpdateDataContext();

        }

        private void UpdateDataContext()
        {
            _value = new Transform(
                new Matrix4x4(
                    Matrix.M11, Matrix.M12, Matrix.M13, Matrix.M14,
                    Matrix.M21, Matrix.M22, Matrix.M23, Matrix.M24,
                    Matrix.M31, Matrix.M32, Matrix.M33, Matrix.M34,
                    Matrix.M41, Matrix.M42, Matrix.M43, Matrix.M44
                ),
                Rotation.Value,
                Translation.Value
            );

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Transform)));
        }
    }
}