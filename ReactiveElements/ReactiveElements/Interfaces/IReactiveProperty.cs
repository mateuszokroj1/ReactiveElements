using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveElements.Interfaces
{
    public interface IReactiveProperty<T> : IReadonlyReactiveProperty<T>
    {
        new T Value { get; set; }

        void SetValue(T value);

        void SetValue(object value);
    }
}
