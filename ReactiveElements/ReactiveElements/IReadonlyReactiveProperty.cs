using System;
using System.ComponentModel;

namespace ReactiveElements
{
    public interface IReadonlyReactiveProperty<T> : IObservable<T>, INotifyPropertyChanged, IDisposable
    {
        T Value { get; }

        T GetValue();
    }
}
