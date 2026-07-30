using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Overlord_PackageManager.resources.GUI.ObservableWrappers
{
    /// <summary>
    /// A wrapper that makes any value observable for WPF binding.
    /// Notifies when the value changes.
    /// </summary>
    public class ObservableValue<T> : INotifyPropertyChanged
    {
        private T _value;

        public ObservableValue(T initialValue)
        {
            _value = initialValue;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Implicit conversion operators
        public static implicit operator T(ObservableValue<T> observable) => observable.Value;
        public static implicit operator ObservableValue<T>(T value) => new ObservableValue<T>(value);

        public override string ToString() => _value?.ToString() ?? string.Empty;
    }
}