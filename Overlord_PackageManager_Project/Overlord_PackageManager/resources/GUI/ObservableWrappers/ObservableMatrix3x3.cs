using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    public class ObservableMatrix3x3 : ObservableComposite
    {
        private Matrix3x3 _value;

        public ObservableValue<float> M11 { get; }
        public ObservableValue<float> M12 { get; }
        public ObservableValue<float> M13 { get; }
        public ObservableValue<float> M21 { get; }
        public ObservableValue<float> M22 { get; }
        public ObservableValue<float> M23 { get; }
        public ObservableValue<float> M31 { get; }
        public ObservableValue<float> M32 { get; }
        public ObservableValue<float> M33 { get; }

        public Matrix3x3 Value => _value;

        public ObservableMatrix3x3(Matrix3x3 initial)
        {
            _value = initial;

            M11 = new(initial.M11);
            M12 = new(initial.M12);
            M13 = new(initial.M13);

            M21 = new(initial.M21);
            M22 = new(initial.M22);
            M23 = new(initial.M23);

            M31 = new(initial.M31);
            M32 = new(initial.M32);
            M33 = new(initial.M33);

            Subscribe(
                M11, M12, M13,
                M21, M22, M23,
                M31, M32, M33);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObservableValue<float>.Value))
                return;

            _value = new Matrix3x3(
                M11.Value, M12.Value, M13.Value,
                M21.Value, M22.Value, M23.Value,
                M31.Value, M32.Value, M33.Value);

            OnPropertyChanged(nameof(Value));
        }
    }
}