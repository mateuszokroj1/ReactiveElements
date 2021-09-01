using System;
using System.Collections.Generic;

namespace ReactiveElements.Observable
{
    internal class Unsubscriber<T> : IDisposable
    {
        #region Constructors

        /// <exception cref="ArgumentNullException" />
        public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
        {
            this.observers = observers ?? throw new ArgumentNullException(nameof(observers));
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        #endregion

        #region Fields

        private readonly IList<IObserver<T>> observers;
        private readonly IObserver<T> observer;
        private bool disposedValue;

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.observers.Contains(this.observer))
                        this.observers.Remove(this.observer);
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
