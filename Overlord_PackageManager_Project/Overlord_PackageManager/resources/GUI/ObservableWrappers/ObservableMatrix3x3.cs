using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableMatrix3x3 : INotifyPropertyChanged
    {
        private Matrix3x3 _value;

        // Individual float properties for binding
        public ObservableValue<float> M11 { get;}
        public ObservableValue<float> M12 { get; }
        public ObservableValue<float> M13 { get; }
        public ObservableValue<float> M21 { get; }
        public ObservableValue<float> M22 { get; }
        public ObservableValue<float> M23 { get; }
        public ObservableValue<float> M31 { get; }
        public ObservableValue<float> M32 { get; }
        public ObservableValue<float> M33 { get; }
        
        public Matrix3x3 Matrix => _value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableMatrix3x3(Matrix3x3 initial)
        {
            _value = initial;
            M11 = new ObservableValue<float>(initial.M11);
            M12 = new ObservableValue<float>(initial.M12);
            M13 = new ObservableValue<float>(initial.M13);
            M21 = new ObservableValue<float>(initial.M21);
            M22 = new ObservableValue<float>(initial.M22);
            M23 = new ObservableValue<float>(initial.M23);
            M31 = new ObservableValue<float>(initial.M31);
            M32 = new ObservableValue<float>(initial.M32);
            M33 = new ObservableValue<float>(initial.M33);

            // Keep the Matrix3x3 updated when any component changes
            M11.PropertyChanged += (s, e) => UpdateMatrix();
            M12.PropertyChanged += (s, e) => UpdateMatrix();
            M13.PropertyChanged += (s, e) => UpdateMatrix();
            M21.PropertyChanged += (s, e) => UpdateMatrix();
            M22.PropertyChanged += (s, e) => UpdateMatrix();
            M23.PropertyChanged += (s, e) => UpdateMatrix();
            M31.PropertyChanged += (s, e) => UpdateMatrix();
            M32.PropertyChanged += (s, e) => UpdateMatrix();
            M33.PropertyChanged += (s, e) => UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            _value = new Matrix3x3(
                M11, M12, M13,
                M21, M22, M23,
                M31, M32, M33
            );
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Matrix)));
        }
    }
}