using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableQuaternion : INotifyPropertyChanged
    {
        private Quaternion _value;

        // Individual float properties for binding
        public ObservableValue<float> X { get;}
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }
        public ObservableValue<float> W { get; }

        public Quaternion Value => _value;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableQuaternion(Quaternion initial)
        {
            _value = initial;
            X = new ObservableValue<float>(initial.X);
            Y = new ObservableValue<float>(initial.Y);
            Z = new ObservableValue<float>(initial.Z);
            W = new ObservableValue<float>(initial.W);

            // Keep the Quaternion updated when any component changes
            X.PropertyChanged += (s, e) => UpdateQuaternion();
            Y.PropertyChanged += (s, e) => UpdateQuaternion();
            Z.PropertyChanged += (s, e) => UpdateQuaternion();
            W.PropertyChanged += (s, e) => UpdateQuaternion();
        }

        private void UpdateQuaternion()
        {
            _value = new Quaternion(X.Value, Y.Value, Z.Value, W.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}