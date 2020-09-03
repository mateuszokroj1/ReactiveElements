using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;


using ReactiveElements.Observable;

namespace ReactiveElements
{

    [Bindable(true, BindingDirection.OneWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value))]
    public class ReadonlyReactiveProperty<T> : IReadonlyReactiveProperty<T>
    {
        #region Fields

        internal T value;
        private bool disposedValue;
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly IDisposable observableSourceUnsubscriber;
        private readonly IObservable<T> observableSource;

        /// <summary>
        /// Event raised when a Value property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

        #region IObservable

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (!this.observers.Contains(observer))
                this.observers.Add(observer);

            observer.OnNext(this.value);

            return new Unsubscriber<T>(this.observers, observer);
        }

        #endregion

        #region IDisposable

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
            //GC.SuppressFinalize(this);
        }

        #endregion

        #region IConvertable

        public TypeCode GetTypeCode() => TypeCode.Object;

        public bool ToBoolean(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public char ToChar(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString();
        }

        /// <exception cref="InvalidOperationException"/>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (Value is IConvertible toConvert)
                return toConvert.ToType(conversionType, provider);
            else if (Value.GetType() == conversionType)
                return Value;
            else
                throw new InvalidOperationException("Cannot convert to: " + conversionType.FullName);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (dynamic)Value;
        }

        #endregion

        #region IObserver

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

        #endregion

        public static implicit operator T(ReadonlyReactiveProperty<T> readonlyReactiveProperty)
        => readonlyReactiveProperty.Value;

        #endregion
    }
}
