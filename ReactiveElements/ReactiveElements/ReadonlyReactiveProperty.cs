using System;
using System.Collections.Generic;
using System.ComponentModel;

using ReactiveElements.Converters;
using ReactiveElements.Observable;

namespace ReactiveElements
{
    [TypeConverter(typeof(PropertyTypeConverter))]
    public class ReadonlyReactiveProperty<T> : IReadonlyReactiveProperty<T>
    {
        #region Fields

        internal T value;
        private bool disposedValue;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly IDisposable observableSourceUnsubscriber;

        #endregion

        #region Constructors

        public ReadonlyReactiveProperty() : this(default(T)) { }

        public ReadonlyReactiveProperty(T value)
        {
            this.value = value;
            this.observableSourceUnsubscriber = null;
        }

        public ReadonlyReactiveProperty(IObservable<T> observableSource)
        {
            if (observableSource == null)
                throw new ArgumentNullException(nameof(observableSource));

            this.observableSourceUnsubscriber = observableSource.Subscribe(args => SetValue(args));
        }

        #endregion

        #region Properties

        public virtual T Value
        {
            get => GetValue();
        }

        #endregion

        #region Methods

        private void NotifySubscribers()
        {
            foreach (IObserver<T> observer in this.observers)
            {
                observer.OnNext(this.value);
                observer.OnCompleted();
            }
        }

        public T GetValue() => this.value;

        protected virtual void SetValue(T value)
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
            if (value.GetType() != typeof(T))
                throw new ArgumentException($"Bad type. Required type {nameof(T)}.");

            SetValue((dynamic)value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (this.observers.Contains(observer))
                return null;

            this.observers.Add(observer);

            observer.OnNext(this.value);
            observer.OnCompleted();

            return new Unsubscriber<T>(this.observers, observer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.observableSourceUnsubscriber?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static implicit operator T(ReadonlyReactiveProperty<T> readonlyReactiveProperty)
        => readonlyReactiveProperty.Value;

        #endregion
    }
}
