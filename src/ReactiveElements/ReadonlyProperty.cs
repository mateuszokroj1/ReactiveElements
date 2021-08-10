using System;
using System.Collections.Generic;
using System.ComponentModel;

using ReactiveElements.Observable;

namespace ReactiveElements
{
    /// <summary>
    /// Reactive property with read-only access
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Bindable(true, BindingDirection.OneWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value))]
    public class ReadonlyProperty<T> : IReadonlyProperty<T>
    {
        #region Fields

        internal T? value;
        private bool disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IList<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly IDisposable? observableSourceUnsubscriber;
        private readonly IObservable<T>? observableSource;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize <see cref="ReadonlyProperty{T}"/> with default value of data type
        /// </summary>
        public ReadonlyProperty() : this(default(T)!) { }

        /// <summary>
        /// Initialize <see cref="ReadonlyProperty{T}"/> with given init value
        /// </summary>
        /// <param name="value">Init value</param>
        public ReadonlyProperty(T? value)
        {
            this.value = value;
            this.observableSourceUnsubscriber = null;
            this.observableSource = null;
        }

        /// <summary>
        /// Initialize <see cref="ReadonlyProperty{T}"/> with given <see cref="IObservable{T}"/> value source
        /// </summary>
        /// <param name="observableSource">Value source</param>
        /// <exception cref="ArgumentNullException" />
        public ReadonlyProperty(IObservable<T> observableSource)
        {
            this.observableSource = observableSource ?? throw new ArgumentNullException(nameof(observableSource));
            this.observableSourceUnsubscriber = this.observableSource.Subscribe(this);
        }

        #endregion

        #region Properties

        [Bindable(true, BindingDirection.OneWay)]
        public virtual T? Value => GetValue();

        #endregion

        #region Methods

        private void NotifySubscribers()
        {
            foreach (var observer in this.observers)
                observer.OnNext(this.value!);
        }

        public T? GetValue() => this.value;

        protected virtual void SetValue(T? value)
        {
            if (!Equals(this.value, value))
            {
                this.value = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                NotifySubscribers();
            }
        }

        /// <exception cref="ArgumentException" />
        protected virtual void SetValue(object value)
        {
            if (value is not T)
                throw new ArgumentException($"Bad type. Required type {nameof(T)}.");

            SetValue((T)value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);

            observer.OnNext(this.value!);

            return new Unsubscriber<T>(this.observers, observer);
        }

        public TypeCode GetTypeCode() => TypeCode.Object;

        public void OnCompleted()
        {
            foreach (var observer in this.observers)
                observer.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var observer in this.observers)
                observer.OnError(error);
        }

        public void OnNext(T value) => SetValue(value);

        public static implicit operator T(ReadonlyProperty<T> readonlyReactiveProperty) => readonlyReactiveProperty.Value!;

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
    }
}
