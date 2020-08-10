using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Text;

using ReactiveElements.Converters;
using ReactiveElements.Observable;

namespace ReactiveElements
{
    [TypeConverter(typeof(PropertyTypeConverter))]
    public sealed class ReactiveProperty<T> : IObservable<T>, INotifyPropertyChanged, IDisposable
    {
        #region Fields

        internal T value;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly IDisposable observableSourceUnsubscriber;
        private readonly TimedValueComparer<T> timedValueComparer;

        #endregion

        #region Constructors

        public ReactiveProperty() : this(default) { }

        public ReactiveProperty(T value)
        {
            this.value = value;
            this.observableSourceUnsubscriber = null;
        }

        public ReactiveProperty(T startValue, IObservable<T> observableSource)
        {
            this.value = startValue;

            if (observableSource == null)
                throw new ArgumentNullException(nameof(observableSource));

            this.observableSourceUnsubscriber = observableSource.Subscribe(args => Value = args);
        }

        public ReactiveProperty(Func<T> sourceFunction, TimeSpan periodOfChecking)
        {
            if(sourceFunction == null)
                if (periodOfChecking > TimeSpan.Zero)
                    
             this.value = sourceFunction();

            this.timedValueComparer = new TimedValueComparer<T>(sourceFunction, periodOfChecking);
            this.observableSourceUnsubscriber = this.timedValueComparer.Subscribe(value => Value = value);
        }

        #endregion

        #region Properties

        public T Value
        {
            get => this.value;
            set
            {
                if(!Equals(this.value, value))
                {
                    this.value = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

                    NotifySubscribers();
                }
            }
        }

        #endregion

        #region Methods

        private void NotifySubscribers()
        {
            foreach(IObserver<T> observer in this.observers)
            {
                observer.OnNext(this.value);
                observer.OnCompleted();
            }
        }

        public T GetValue() => Value;

        public void SetValue(T value)
        {
            Value = value;
        }

        /// <exception cref="ArgumentException" />
        public void SetValue(object value)
        {
            if (value.GetType() != typeof(T))
                throw new ArgumentException($"Bad type. Required type {nameof(T)}.");

            Value = (dynamic)value;
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

        public void Dispose()
        {
            this.observableSourceUnsubscriber?.Dispose();
            this.timedValueComparer?.Dispose();
        }

        ~ReactiveProperty() => Dispose();

        public static ReactiveProperty<T> FromBindableModel<TModel>(TModel model, Expression<Func<TModel, T>> propertySelectionExpression)
            where TModel : INotifyPropertyChanged
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model.GetReactiveProperty(propertySelectionExpression);
        }

        #endregion
    }
}
