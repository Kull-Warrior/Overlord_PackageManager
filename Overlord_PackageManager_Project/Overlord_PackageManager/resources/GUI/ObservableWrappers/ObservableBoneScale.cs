using Overlord_PackageManager.resources.Data.DataTypes;
using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    // Observable wrapper for UI binding
    public class ObservableBoneScale : ObservableComposite
    {
        private BoneScale _value;

        public ObservableValue<Half> ScaleX { get;}
        public ObservableValue<Half> ScaleY { get; }
        public ObservableValue<Half> ScaleZ { get; }

        public BoneScale Value => _value;
        
        public ObservableBoneScale(BoneScale initial)
        {
            _value = initial;

            ScaleX = new(initial.ScaleX);
            ScaleY = new(initial.ScaleY);
            ScaleZ = new(initial.ScaleZ);

            // Keep the BoneScale updated when any component changes
            Subscribe(ScaleX, ScaleY, ScaleZ);
        }

        protected override void OnComponentChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Value))
                return;

            _value = new BoneScale(ScaleX.Value, ScaleY.Value, ScaleZ.Value);
            OnPropertyChanged(nameof(Value));
        }
    }
}