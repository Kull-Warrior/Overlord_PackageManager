using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableObjectBone : INotifyPropertyChanged
    {
        private ObjectBone _value;

        // Individual float properties for binding

        public ObservableValue<char[]> Name { get; }

        public ObservableTransform Transform { get; }

        public ObservableValue<int> ParentIndex { get; }

        public ObservableValue<int> NextSiblingIndex { get; }

        public ObservableValue<int> NextTraversalIndex { get; }

        public ObservableValue<int> Reserved { get; }

        public ObjectBone Value => _value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableObjectBone(ObjectBone initial)
        {
            _value = initial;

            Name = new ObservableValue<char[]>(initial.Name);
            Transform = new ObservableTransform(initial.Transform);
            ParentIndex = new ObservableValue<int>(initial.ParentIndex);
            NextSiblingIndex = new ObservableValue<int>(initial.NextSiblingIndex);
            NextTraversalIndex = new ObservableValue<int>(initial.NextTraversalIndex);
            Reserved = new ObservableValue<int>(initial.Reserved);

            // Keep the Transform updated when any component changes
            Transform.PropertyChanged += (s, e) => UpdateDataContext();
            ParentIndex.PropertyChanged += (s, e) => UpdateDataContext();
            NextSiblingIndex.PropertyChanged += (s, e) => UpdateDataContext();
            NextTraversalIndex.PropertyChanged += (s, e) => UpdateDataContext();
            Reserved.PropertyChanged += (s, e) => UpdateDataContext();
        }

        private void UpdateDataContext()
        {
            _value = new ObjectBone(
                Name.Value,
                new Transform(
                    new Matrix4x4(
                        Transform.Matrix.M11, Transform.Matrix.M12, Transform.Matrix.M13, Transform.Matrix.M14,
                        Transform.Matrix.M21, Transform.Matrix.M22, Transform.Matrix.M23, Transform.Matrix.M24,
                        Transform.Matrix.M31, Transform.Matrix.M32, Transform.Matrix.M33, Transform.Matrix.M34,
                        Transform.Matrix.M41, Transform.Matrix.M42, Transform.Matrix.M43, Transform.Matrix.M44
                    ),
                    Transform.Rotation.Value,
                    Transform.Translation.Value
                ),
                ParentIndex.Value,
                NextSiblingIndex.Value,
                NextTraversalIndex.Value,
                Reserved.Value
            );

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ObjectBone)));
        }
    }
}