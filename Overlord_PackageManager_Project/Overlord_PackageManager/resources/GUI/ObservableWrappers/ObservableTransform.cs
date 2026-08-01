using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableTransform : ObservableComposite
    {
        private Transform _value;

        public ObservableMatrix4x4 Matrix { get; }
        public ObservableQuaternion Rotation { get; }
        public ObservableVector4 Translation { get; }

        public Transform Value => _value;
        
        public ObservableTransform(Transform initial)
        {
            _value = initial;

            Matrix = new (initial.Matrix);
            Rotation = new (initial.Rotation);
            Translation = new (initial.Translation);

            // Keep the Transform updated when any component changes
            Subscribe(Matrix, Rotation, Translation);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new Transform(
                Matrix.Value,
                Rotation.Value,
                Translation.Value
            );

            OnPropertyChanged(nameof(Value));
        }
    }
}