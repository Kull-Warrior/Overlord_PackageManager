using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableRawMeshClusterData : ObservableComposite
    {
        private RawMeshClusterData _value;

        public ObservableMatrix3x3 Matrix { get; }
        public ObservableVector3 Head { get; }
        public ObservableVector3 Tail { get; }
        public ObservableValue<ushort> PatchIndex {  get; }
        public ObservableValue<ushort> TriangleCount {  get; }

        public RawMeshClusterData Value => _value;
        
        public ObservableRawMeshClusterData(RawMeshClusterData initial)
        {
            _value = initial;

            Matrix = new (initial.Matrix);
            Head = new (initial.Center);
            Tail = new (initial.Extents);
            PatchIndex = new (initial.patchIndex);
            TriangleCount = new (initial.triangleCount);

            // Keep the RawMeshClusterData updated when any component changes
            Subscribe(Matrix, Head, Tail, PatchIndex, TriangleCount);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new RawMeshClusterData(
                Matrix.Value,
                Head.Value,
                Tail.Value,
                PatchIndex.Value,
                TriangleCount.Value
            );

            OnPropertyChanged(nameof(Value));
        }
    }
}