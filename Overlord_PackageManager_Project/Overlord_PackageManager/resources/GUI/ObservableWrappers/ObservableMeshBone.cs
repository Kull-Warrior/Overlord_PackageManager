using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableMeshBone : ObservableComposite
    {
        private MeshBone _value;

        public ObservableValue<char[]> Name { get; }
        public ObservableMeshTransform Transform { get; }
        public ObservableValue<int> Unknown1 { get; }
        public ObservableValue<int> Unknown2 { get; }
        public ObservableValue<int> Unknown3 { get; }
        public ObservableValue<int> Unknown4 { get; }
        public ObservableValue<int> Unknown5 { get; }
        public ObservableValue<int> Unknown6 { get; }

        public MeshBone Value => _value;
        
        public ObservableMeshBone(MeshBone initial)
        {
            _value = initial;

            Name = new (initial.Name);
            Transform = new (initial.Transform);
            Unknown1 = new (initial.Unknown1);
            Unknown2 = new (initial.Unknown2);
            Unknown3 = new (initial.Unknown3);
            Unknown4 = new (initial.Unknown4);
            Unknown5 = new (initial.Unknown5);
            Unknown6 = new (initial.Unknown6);

            // Keep the Transform updated when any component changes
            Subscribe(Name, Transform, Unknown1, Unknown2, Unknown3, Unknown4, Unknown5, Unknown6);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new MeshBone(
                Name.Value,
                Transform.Value,
                Unknown1.Value,
                Unknown2.Value,
                Unknown3.Value,
                Unknown4.Value,
                Unknown5.Value,
                Unknown6.Value
            );

            OnPropertyChanged(nameof(Value));
        }
    }
}