using System.ComponentModel;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    /// <summary>
    /// Base class for observable objects composed of multiple ObservableValue components.
    /// Handles event subscription and PropertyChanged notification.
    /// </summary>
    public abstract class ObservableComposite : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Subscribe(params INotifyPropertyChanged[] observables)
        {
            foreach (var observable in observables)
            {
                observable.PropertyChanged += OnComponentChanged;
            }
        }

        protected abstract void OnComponentChanged(object? sender, PropertyChangedEventArgs e);
    }
}
