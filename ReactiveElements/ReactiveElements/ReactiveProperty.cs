using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using ReactiveElements.Converters;
using ReactiveElements.Observable;

namespace ReactiveElements
{
    [TypeConverter(typeof(PropertyTypeConverter))]
    public class ReactiveProperty<T> : Observable<T>
    {
        #region Fields

        internal T value;
        public PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public ReactiveProperty() : this(default)
        {

        }

        public ReactiveProperty(T value)
        {
            this.value = value;
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


                }
            }
        }

        #endregion
    }
}
