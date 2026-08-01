using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    public class ObservableVector4 : ObservableComposite
    {
        private Vector4 _value;

        public ObservableValue<float> X { get; }
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }
        public ObservableValue<float> W { get; }

        public Vector4 Value => _value;

        public ObservableVector4(Vector4 initial)
        {
            _value = initial;

            X = new(initial.X);
            Y = new(initial.Y);
            Z = new(initial.Z);
            W = new(initial.W);

            Subscribe(X, Y, Z, W);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObservableValue<float>.Value))
                return;

            _value = new Vector4(
                X.Value,
                Y.Value,
                Z.Value,
                W.Value);

            OnPropertyChanged(nameof(Value));
        }
    }
}