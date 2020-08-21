using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace ReactiveElements.Observable
{
    public class TimedValueComparer<T> : ITimedValueComparer<T>
    {
        #region Fields

        private readonly TimeSpan periodOfChecking;
        private T currentValue;
        private readonly Func<T> funcToCheck;
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
        private bool disposedValue;
        private readonly Timer timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="TimedValueComparer{T}"/>, that checks the value of function in setted period.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        /// <exception cref="ExternalException" />
        public TimedValueComparer(Func<T> funcToCheck, TimeSpan periodOfChecking, SynchronizationContext synchronizationContext)
        {
            this.funcToCheck = funcToCheck ?? throw new ArgumentNullException(nameof(funcToCheck));

            if (periodOfChecking <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("Period is too small");

            this.periodOfChecking = periodOfChecking;

            try
            {
                this.currentValue = funcToCheck();
            }
            catch (Exception exc)
            {
                throw new ExternalException("FuncToCheck throws unhandled exception.", exc);
            }

            NotifySubscribers();
            SynchronizationContext = synchronizationContext ?? throw new ArgumentNullException(nameof(synchronizationContext));

            this.timer = new Timer(
                state => SynchronizationContext.Post(args => UpdateValue(), null),
                null,
                TimeSpan.Zero,
                this.periodOfChecking
            );
        }

        /// <summary>
        /// Creates new instance of <see cref="TimedValueComparer{T}"/>, that checks the value of function in setted period.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        /// <exception cref="ExternalException" />
        public TimedValueComparer(Func<T> funcToCheck, TimeSpan periodOfChecking)
            : this(funcToCheck, periodOfChecking, SynchronizationContext.Current) { }

        #endregion

        /// <summary>
        /// Notifies registered observers about new value
        /// </summary>
        private void NotifySubscribers()
        {
            foreach (var observer in this.observers)
                observer.OnNext(this.currentValue);
        }

        public SynchronizationContext SynchronizationContext { get; set; }

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
                var exception = new ExternalException("FuncToCheck throws an unhandled exception.", exc); ;

                foreach (var observer in this.observers)
                    observer.OnError(exception);

                throw exception;
            }

            NotifySubscribers();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);

            observer.OnNext(this.currentValue);

            return new Unsubscriber<T>(this.observers, observer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.timer?.Dispose();

                    foreach (var observer in this.observers)
                        observer.OnCompleted();

                    this.observers.Clear();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
