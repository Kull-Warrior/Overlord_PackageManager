using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    public class ObservableMatrix4x4 : ObservableComposite
    {
        private Matrix4x4 _value;

        public ObservableValue<float> M11 { get; }
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

        public Matrix4x4 Value => _value;

        public ObservableMatrix4x4(Matrix4x4 initial)
        {
            _value = initial;

            M11 = new(initial.M11);
            M12 = new(initial.M12);
            M13 = new(initial.M13);
            M14 = new(initial.M14);

            M21 = new(initial.M21);
            M22 = new(initial.M22);
            M23 = new(initial.M23);
            M24 = new(initial.M24);

            M31 = new(initial.M31);
            M32 = new(initial.M32);
            M33 = new(initial.M33);
            M34 = new(initial.M34);

            M41 = new(initial.M41);
            M42 = new(initial.M42);
            M43 = new(initial.M43);
            M44 = new(initial.M44);

            Subscribe(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObservableValue<float>.Value))
                return;

            _value = new Matrix4x4(
                M11.Value, M12.Value, M13.Value, M14.Value,
                M21.Value, M22.Value, M23.Value, M24.Value,
                M31.Value, M32.Value, M33.Value, M34.Value,
                M41.Value, M42.Value, M43.Value, M44.Value);

            OnPropertyChanged(nameof(Value));
        }
    }
}