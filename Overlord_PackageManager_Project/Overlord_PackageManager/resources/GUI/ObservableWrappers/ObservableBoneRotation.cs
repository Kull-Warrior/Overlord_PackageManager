using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableBoneRotation : INotifyPropertyChanged
    {
        private BoneRotation _value;

        // Individual float properties for binding
        public ObservableValue<float> Yaw { get;}
        public ObservableValue<float> Pitch { get; }
        public ObservableValue<float> Roll { get; }

        public BoneRotation Value => _value;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableBoneRotation(BoneRotation initial)
        {
            _value = initial;

            Yaw = new ObservableValue<float>(initial.Yaw);
            Pitch = new ObservableValue<float>(initial.Pitch);
            Roll = new ObservableValue<float>(initial.Roll);

            // Keep the BoneRotation updated when any component changes
            Yaw.PropertyChanged += (s, e) => UpdateBoneRotation();
            Pitch.PropertyChanged += (s, e) => UpdateBoneRotation();
            Roll.PropertyChanged += (s, e) => UpdateBoneRotation();
        }

        private void UpdateBoneRotation()
        {
            _value = new BoneRotation(Yaw.Value, Pitch.Value, Roll.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}