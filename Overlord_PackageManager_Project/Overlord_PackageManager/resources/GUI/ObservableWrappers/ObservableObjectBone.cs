using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableObjectBone : ObservableComposite
    {
        private ObjectBone _value;

        public ObservableValue<char[]> Name { get; }
        public ObservableTransform Transform { get; }
        public ObservableValue<int> ParentIndex { get; }
        public ObservableValue<int> NextSiblingIndex { get; }
        public ObservableValue<int> NextTraversalIndex { get; }
        public ObservableValue<int> Reserved { get; }

        public ObjectBone Value => _value;
        
        public ObservableObjectBone(ObjectBone initial)
        {
            _value = initial;

            Name = new (initial.Name);
            Transform = new (initial.Transform);
            ParentIndex = new (initial.ParentIndex);
            NextSiblingIndex = new (initial.NextSiblingIndex);
            NextTraversalIndex = new (initial.NextTraversalIndex);
            Reserved = new (initial.Reserved);

            // Keep the Transform updated when any component changes
            Subscribe(Name, Transform, ParentIndex, NextSiblingIndex, NextTraversalIndex, Reserved);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new ObjectBone(
                Name.Value,
                Transform.Value,
                ParentIndex.Value,
                NextSiblingIndex.Value,
                NextTraversalIndex.Value,
                Reserved.Value
            );

            OnPropertyChanged(nameof(Value));
        }
    }
}