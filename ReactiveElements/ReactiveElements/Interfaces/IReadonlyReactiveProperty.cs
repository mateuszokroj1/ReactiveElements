using System;
using System.ComponentModel;

namespace ReactiveElements.Interfaces
{
    public interface IReadonlyReactiveProperty<T> : IObservable<T>, INotifyPropertyChanged, IDisposable
    {
        T Value { get; }

        T GetValue();
    }
}
