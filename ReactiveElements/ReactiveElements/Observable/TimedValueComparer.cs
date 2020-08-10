using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ReactiveElements.Observable
{
    internal class TimedValueComparer<T> : IObservable<T>, IDisposable
    {
        private TimeSpan periodOfChecking;
        private T currentValue;
        private Func<T> funcToCheck;
        private List<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly Timer timer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcToCheck"></param>
        /// <param name="periodOfChecking"></param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        /// <exception cref="ExternalException" />
        public TimedValueComparer(Func<T> funcToCheck, TimeSpan periodOfChecking)
        {
            this.funcToCheck = funcToCheck ?? throw new ArgumentNullException(nameof(funcToCheck));

            if (periodOfChecking <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("Period is too small");

            this.periodOfChecking = periodOfChecking;

            try
            {
                this.currentValue = funcToCheck();
            }
            catch(Exception exc)
            {
                throw new ExternalException("FuncToCheck throws unhandled exception.", exc);
            }

            NotifySubscribers();

            this.timer = new Timer(
                state => SynchronizationContext.Current.Post(args => UpdateValue(), null),
                null,
                TimeSpan.Zero,
                this.periodOfChecking
            );
        }

        public void Dispose()
        {
            this.timer?.Dispose();

        }

        /// <summary>
        /// Notifies registered observers about new value
        /// </summary>
        private void NotifySubscribers()
        {
            foreach(var observer in this.observers)
            {
                observer.OnNext(this.currentValue);
                observer.OnCompleted();
            }
        }

        /// <summary>
        /// Updates value with FuncToCheck
        /// </summary>
        /// <exception cref="ExternalException" />
        private void UpdateValue()
        {
            try
            {
                this.currentValue = this.funcToCheck();
            }
            catch (Exception exc)
            {
                throw new ExternalException("FuncToCheck throws an unhandled exception.", exc);
            }

            NotifySubscribers();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (this.observers.Contains(observer))
                return null;

            this.observers.Add(observer);

            observer.OnNext(this.currentValue);
            observer.OnCompleted();

            return new Unsubscriber<T>(this.observers, observer);
        }
    }
}
