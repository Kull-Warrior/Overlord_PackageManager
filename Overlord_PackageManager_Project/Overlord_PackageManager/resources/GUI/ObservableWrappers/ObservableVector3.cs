using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableVector3 : INotifyPropertyChanged
    {
        private Vector3 _value;

        // Individual float properties for binding
        public ObservableValue<float> X { get;}
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }
        
        public Vector3 Value => _value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableVector3(Vector3 initial)
        {
            _value = initial;
            X = new ObservableValue<float>(initial.X);
            Y = new ObservableValue<float>(initial.Y);
            Z = new ObservableValue<float>(initial.Z);

            // Keep the Vector3 updated when any component changes
            X.PropertyChanged += (s, e) => UpdateVector();
            Y.PropertyChanged += (s, e) => UpdateVector();
            Z.PropertyChanged += (s, e) => UpdateVector();
        }

        private void UpdateVector()
        {
            _value = new Vector3(X.Value, Y.Value, Z.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}