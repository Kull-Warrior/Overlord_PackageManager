using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableMatrix4x4 : INotifyPropertyChanged
    {
        private Matrix4x4 _value;

        // Individual float properties for binding
        public ObservableValue<float> M11 { get;}
        public ObservableValue<float> M12 { get; }
        public ObservableValue<float> M13 { get; }
        public ObservableValue<float> M14 { get; }
        public ObservableValue<float> M21 { get; }
        public ObservableValue<float> M22 { get; }
        public ObservableValue<float> M23 { get; }
        public ObservableValue<float> M24 { get; }
        public ObservableValue<float> M31 { get; }
        public ObservableValue<float> M32 { get; }
        public ObservableValue<float> M33 { get; }
        public ObservableValue<float> M34 { get; }  
        public ObservableValue<float> M41 { get; }
        public ObservableValue<float> M42 { get; }
        public ObservableValue<float> M43 { get; }
        public ObservableValue<float> M44 { get; }

        public Matrix4x4 Matrix => _value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableMatrix4x4(Matrix4x4 initial)
        {
            _value = initial;
            M11 = new ObservableValue<float>(initial.M11);
            M12 = new ObservableValue<float>(initial.M12);
            M13 = new ObservableValue<float>(initial.M13);
            M14 = new ObservableValue<float>(initial.M14);
            M21 = new ObservableValue<float>(initial.M21);
            M22 = new ObservableValue<float>(initial.M22);
            M23 = new ObservableValue<float>(initial.M23);
            M24 = new ObservableValue<float>(initial.M24);
            M31 = new ObservableValue<float>(initial.M31);
            M32 = new ObservableValue<float>(initial.M32);
            M33 = new ObservableValue<float>(initial.M33);
            M34 = new ObservableValue<float>(initial.M34);
            M41 = new ObservableValue<float>(initial.M41);
            M42 = new ObservableValue<float>(initial.M42);
            M43 = new ObservableValue<float>(initial.M43);
            M44 = new ObservableValue<float>(initial.M44);

            // Keep the Matrix4x4 updated when any component changes
            M11.PropertyChanged += (s, e) => UpdateMatrix();
            M12.PropertyChanged += (s, e) => UpdateMatrix();
            M13.PropertyChanged += (s, e) => UpdateMatrix();
            M14.PropertyChanged += (s, e) => UpdateMatrix();
            M21.PropertyChanged += (s, e) => UpdateMatrix();
            M22.PropertyChanged += (s, e) => UpdateMatrix();
            M23.PropertyChanged += (s, e) => UpdateMatrix();
            M24.PropertyChanged += (s, e) => UpdateMatrix();
            M31.PropertyChanged += (s, e) => UpdateMatrix();
            M32.PropertyChanged += (s, e) => UpdateMatrix();
            M33.PropertyChanged += (s, e) => UpdateMatrix();
            M34.PropertyChanged += (s, e) => UpdateMatrix();
            M41.PropertyChanged += (s, e) => UpdateMatrix();
            M42.PropertyChanged += (s, e) => UpdateMatrix();
            M43.PropertyChanged += (s, e) => UpdateMatrix();
            M44.PropertyChanged += (s, e) => UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            _value = new Matrix4x4(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44
            );
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Matrix)));
        }
    }
}