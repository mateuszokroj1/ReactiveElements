using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveElements.Observable
{
    public abstract class Observable<T> : IObservable<T>
    {
        private List<IObserver<T>> observers = new List<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            
        }
    }
}
