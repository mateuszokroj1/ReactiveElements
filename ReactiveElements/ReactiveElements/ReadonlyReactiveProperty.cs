using System;
using System.Collections.Generic;
using System.ComponentModel;

using ReactiveElements.Observable;

namespace ReactiveElements
{
    [Bindable(true, BindingDirection.OneWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value))]
    public class ReadonlyReactiveProperty<T> : IReadonlyReactiveProperty<T>
    {
        #region Fields

        internal T value;
        private bool disposedValue;

        /// <summary>
        /// Notifies about changes in properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly IDisposable observableSourceUnsubscriber;
        private readonly IObservable<T> observableSource;

        #endregion

        #region Constructors

        public ReadonlyReactiveProperty() : this(default(T)) { }

        public ReadonlyReactiveProperty(T value)
        {
            this.value = value;
            this.observableSourceUnsubscriber = null;
            this.observableSource = null;
        }

        public ReadonlyReactiveProperty(IObservable<T> observableSource)
        {
            this.observableSource = observableSource ?? throw new ArgumentNullException(nameof(observableSource));
            this.observableSourceUnsubscriber = this.observableSource.Subscribe(this);
        }

        #endregion

        #region Properties

        [Bindable(true, BindingDirection.OneWay)]
        public virtual T Value
        {
            get => GetValue();
        }

        #endregion

        #region Methods

        private void NotifySubscribers()
        {
            foreach (var observer in this.observers)
                observer.OnNext(this.value);
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
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);

            observer.OnNext(this.value);

            return new Unsubscriber<T>(this.observers, observer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.observableSourceUnsubscriber?.Dispose();

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

        public static implicit operator T(ReadonlyReactiveProperty<T> readonlyReactiveProperty)
        => readonlyReactiveProperty.Value;

        #endregion
    }
}
