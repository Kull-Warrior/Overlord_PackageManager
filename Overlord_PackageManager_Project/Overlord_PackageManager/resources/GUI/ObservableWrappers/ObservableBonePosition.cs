using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableBonePosition : ObservableComposite
    {
        private BonePosition _value;

        public ObservableValue<uint> Timestamp { get; set; }
        public ObservableValue<float> X { get;}
        public ObservableValue<float> Y { get; }
        public ObservableValue<float> Z { get; }

        public BonePosition Value => _value;
        
        public ObservableBonePosition(BonePosition initial)
        {
            _value = initial;

            Timestamp = new(initial.Timestamp);
            X = new(initial.X);
            Y = new(initial.Y);
            Z = new(initial.Z);

            // Keep the BonePosition updated when any component changes
            Subscribe(Timestamp, X, Y, Z);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new BonePosition(Timestamp.Value, X.Value, Y.Value, Z.Value);
            OnPropertyChanged(nameof(Value));
        }
    }
}