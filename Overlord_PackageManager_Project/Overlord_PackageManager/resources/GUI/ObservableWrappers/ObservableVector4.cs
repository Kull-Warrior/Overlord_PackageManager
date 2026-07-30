using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableVector4 : INotifyPropertyChanged
    {
        private Vector4 _value;

        // Individual float properties for binding
        public ObservableValue<float> X { get;}
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }
        public ObservableValue<float> W { get; }

        public Vector4 Value => _value;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableVector4(Vector4 initial)
        {
            _value = initial;
            X = new ObservableValue<float>(initial.X);
            Y = new ObservableValue<float>(initial.Y);
            Z = new ObservableValue<float>(initial.Z);
            W = new ObservableValue<float>(initial.W);

            // Keep the Vector4 updated when any component changes
            X.PropertyChanged += (s, e) => UpdateVector();
            Y.PropertyChanged += (s, e) => UpdateVector();
            Z.PropertyChanged += (s, e) => UpdateVector();
            W.PropertyChanged += (s, e) => UpdateVector();
        }

        private void UpdateVector()
        {
            _value = new Vector4(X.Value, Y.Value, Z.Value, W.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}