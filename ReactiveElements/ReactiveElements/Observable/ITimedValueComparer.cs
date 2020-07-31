using System;
using System.Threading;

namespace ReactiveElements.Observable
{
    public interface ITimedValueComparer<T> : IObservable<T>, IDisposable
    {
        SynchronizationContext SynchronizationContext { get; set; }
    }
}
