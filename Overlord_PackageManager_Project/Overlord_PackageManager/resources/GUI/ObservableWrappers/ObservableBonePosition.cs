using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;
using System.Numerics;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableBonePosition : INotifyPropertyChanged
    {
        private BonePosition _value;

        // Individual float properties for binding

        public ObservableValue<uint> Timestamp { get; set; }
        public ObservableValue<float> X { get;}
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }

        public BonePosition Value => _value;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableBonePosition(BonePosition initial)
        {
            _value = initial;

            Timestamp = new ObservableValue<uint>(initial.Timestamp);
            X = new ObservableValue<float>(initial.X);
            Y = new ObservableValue<float>(initial.Y);
            Z = new ObservableValue<float>(initial.Z);

            // Keep the BonePosition updated when any component changes
            Timestamp.PropertyChanged += (s, e) => UpdateBonePosition();
            X.PropertyChanged += (s, e) => UpdateBonePosition();
            Y.PropertyChanged += (s, e) => UpdateBonePosition();
            Z.PropertyChanged += (s, e) => UpdateBonePosition();
        }

        private void UpdateBonePosition()
        {
            _value = new BonePosition(Timestamp.Value, X.Value, Y.Value, Z.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}