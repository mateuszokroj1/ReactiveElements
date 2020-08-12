using System;
using System.Threading;

namespace ReactiveElements.Interfaces
{
    public interface ITimedValueComparer<T> : IObservable<T>, IDisposable
    {
        SynchronizationContext SynchronizationContext { get; set; }
    }
}
