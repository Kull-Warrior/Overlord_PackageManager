using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableQuaternion : ObservableComposite
    {
        private Quaternion _value;

        // Individual float properties for binding
        public ObservableValue<float> X { get;}
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }
        public ObservableValue<float> W { get; }

        public Quaternion Value => _value;
        
        public ObservableQuaternion(Quaternion initial)
        {
            _value = initial;
            X = new (initial.X);
            Y = new (initial.Y);
            Z = new (initial.Z);
            W = new (initial.W);

            // Keep the Quaternion updated when any component changes
            Subscribe(X, Y, Z, W);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new Quaternion(X.Value, Y.Value, Z.Value, W.Value);
            OnPropertyChanged(nameof(Value));
        }
    }
}