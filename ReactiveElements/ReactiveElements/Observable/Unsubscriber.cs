using System;
using System.Collections.Generic;

namespace ReactiveElements.Observable
{
    internal class Unsubscriber<T> : IDisposable
    {
        #region Fields

        private readonly IList<IObserver<T>> observers;
        private readonly IObserver<T> observer;

        #endregion

        #region Constructors

        public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
        {
            this.observers = observers ?? throw new ArgumentNullException(nameof(observers));
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (this.observers.Contains(this.observer))
                this.observers.Remove(this.observer);
        }

        ~Unsubscriber()
        {
            Dispose();
        }

        #endregion
    }
}
