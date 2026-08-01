using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableBoneRotation : ObservableComposite
    {
        private BoneRotation _value;

        public ObservableValue<float> Yaw { get;}
        public ObservableValue<float> Pitch { get; }
        public ObservableValue<float> Roll { get; }

        public BoneRotation Value => _value;
        
        public ObservableBoneRotation(BoneRotation initial)
        {
            _value = initial;

            Yaw = new ObservableValue<float>(initial.Yaw);
            Pitch = new ObservableValue<float>(initial.Pitch);
            Roll = new ObservableValue<float>(initial.Roll);

            // Keep the BoneRotation updated when any component changes
            Subscribe(Yaw, Pitch, Roll);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new BoneRotation(Yaw.Value, Pitch.Value, Roll.Value);
            OnPropertyChanged(nameof(Value));
        }
    }
}