using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    public class ObservableVector3 : ObservableComposite
    {
        private Vector3 _value;

        public ObservableValue<float> X { get; }
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }

        public Vector3 Value => _value;

        public ObservableVector3(Vector3 initial)
        {
            _value = initial;

            X = new(initial.X);
            Y = new(initial.Y);
            Z = new(initial.Z);

            Subscribe(X, Y, Z);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObservableValue<float>.Value))
                return;

            _value = new Vector3(
                X.Value,
                Y.Value,
                Z.Value);

            OnPropertyChanged(nameof(Value));
        }
    }
}