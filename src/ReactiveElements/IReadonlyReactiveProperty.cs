using System;
using System.ComponentModel;

namespace ReactiveElements
{
    /// <summary>
    /// Interface for ReadonlyReactiveProperty class
    /// </summary>
    /// <typeparam name="T">Type of property value</typeparam>
    public interface IReadonlyReactiveProperty<T> : IObservable<T>, IObserver<T>, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Represents current value of property
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Returns current value of property
        /// </summary>
        T GetValue();
    }
}
