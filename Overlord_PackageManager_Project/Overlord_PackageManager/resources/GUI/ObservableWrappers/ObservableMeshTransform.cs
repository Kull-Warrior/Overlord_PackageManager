using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableMeshTransform : ObservableComposite
    {
        private MeshTransform _value;

        public ObservableMatrix4x4 Matrix { get; }
        public ObservableVector3 Scale { get; }
        public ObservableVector3 Translation { get; }
        public ObservableQuaternion Rotation { get; }

        public MeshTransform Value => _value;
        
        public ObservableMeshTransform(MeshTransform initial)
        {
            _value = initial;

            Matrix = new (initial.Matrix);
            Scale = new(initial.Scale);
            Translation = new (initial.Translation);
            Rotation = new (initial.Rotation);

            // Keep the Transform updated when any component changes
            Subscribe(Matrix, Scale, Translation, Rotation);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new MeshTransform(
                Matrix.Value,
                Scale.Value,
                Translation.Value,
                Rotation.Value
            );

            OnPropertyChanged(nameof(Value));
        }
    }
}