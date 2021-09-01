using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;

using ReactiveElements.Observable;

namespace ReactiveElements
{
    /// <summary>
    /// Reactive property with read-only access
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    [Bindable(true, BindingDirection.OneWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value))]
    public class ReadonlyProperty<T> : IReadonlyProperty<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes <see cref="ReadonlyProperty{T}"/> with default value of <typeparamref name="T"/>.
        /// </summary>
        public ReadonlyProperty() : this(default!) { }

        /// <summary>
        /// Initializes <see cref="ReadonlyProperty{T}"/> with given init value.
        /// </summary>
        /// <param name="value">Init value</param>
        public ReadonlyProperty(T? value)
        {
            this.value = value;
            this.observableSourceUnsubscriber = null;
        }

        /// <summary>
        /// Initializes <see cref="ReadonlyProperty{T}"/> with given <see cref="IObservable{T}"/> value source.
        /// </summary>
        /// <param name="observableSource">Value source</param>
        /// <exception cref="ArgumentNullException" />
        public ReadonlyProperty(IObservable<T> observableSource, SynchronizationContext? sourceSynchronization = null)
        {
            if(observableSource is null)
                throw new ArgumentNullException(nameof(observableSource));

            this.observableSourceUnsubscriber = sourceSynchronization is null
                ? observableSource.Subscribe(this)
                : observableSource.ObserveOn(sourceSynchronization).Subscribe(this);
        }

        #endregion

        #region Fields

        internal T? value;
        private bool disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        private readonly IList<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly IDisposable? observableSourceUnsubscriber;

        #endregion

        #region Properties

        [Bindable(true, BindingDirection.OneWay), Browsable(true)]
        public virtual T? Value => GetValue();

        #endregion

        #region Methods

        private void NotifySubscribers()
        {
            foreach (var observer in this.observers)
                observer.OnNext(this.value!);
        }

        public T? GetValue() => this.value;

        /// <summary>
        /// When value is different with current, sets new value and notify observers about change.
        /// </summary>
        protected virtual void SetValue(T? value)
        {
            if (!Equals(this.value, value))
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Value)));

                this.value = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                NotifySubscribers();
            }
        }

        protected virtual void SetValue(object? value)
        {
            if(value is null)
                SetValue(default);
            else
                SetValue((T)Convert.ChangeType(value, typeof(T)));
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);

            observer.OnNext(this.value!);

            return new Unsubscriber<T>(this.observers, observer);
        }

        #region IObservator

        public void OnCompleted()
        {
            foreach (var observer in this.observers)
                observer?.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var observer in this.observers)
                observer.OnError(error);
        }

        public void OnNext(T? value) => SetValue(value);

        #endregion

        public static implicit operator T(ReadonlyProperty<T> readonlyReactiveProperty) => readonlyReactiveProperty.Value!;

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.observableSourceUnsubscriber?.Dispose();

                    foreach (var observer in this.observers)
                        observer?.OnCompleted();

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

        #endregion

        #endregion
    }
}
