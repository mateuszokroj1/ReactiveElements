using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveElements.Observable
{
    internal class TimedValueComparer<T> : IObservable<T>, IDisposable
    {
        private TimeSpan periodOfChecking;
        private Func<T> funcToCheck;
        private List<IObserver<T>> observers = new List<IObserver<T>>();

        public TimedValueComparer(Func<T> funcToCheck, TimeSpan periodOfChecking)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
