using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableBoneScale : INotifyPropertyChanged
    {
        private BoneScale _value;

        // Individual float properties for binding
        public ObservableValue<Half> ScaleX { get;}
        public ObservableValue<Half> ScaleY { get; }
        public ObservableValue<Half> ScaleZ { get; }

        public BoneScale Value => _value;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableBoneScale(BoneScale initial)
        {
            _value = initial;

            ScaleX = new ObservableValue<Half>(initial.ScaleX);
            ScaleY = new ObservableValue<Half>(initial.ScaleY);
            ScaleZ = new ObservableValue<Half>(initial.ScaleZ);

            // Keep the BoneScale updated when any component changes
            ScaleX.PropertyChanged += (s, e) => UpdateBoneScale();
            ScaleY.PropertyChanged += (s, e) => UpdateBoneScale();
            ScaleZ.PropertyChanged += (s, e) => UpdateBoneScale();
        }

        private void UpdateBoneScale()
        {
            _value = new BoneScale(ScaleX.Value, ScaleY.Value, ScaleZ.Value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}