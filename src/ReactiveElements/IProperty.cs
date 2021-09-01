﻿namespace ReactiveElements
{
    public interface IProperty<T> : IReadonlyProperty<T>
    {
        /// <summary>
        /// Represents current value of property
        /// </summary>
        new T? Value { get; set; }

        /// <summary>
        /// When value is different with current, sets new value and notify observers about change.
        /// </summary>
        void SetValue(T? value);

        /// <summary>
        /// <para>Set value from object using <see cref="System.Convert"/>.</para>
        /// <para>When value is null and <typeparamref name="T"/> is <see langword="struct"/>, then sets default value.</para>
        /// </summary>
        /// <exception cref="System.InvalidCastException" />
        /// <exception cref="System.FormatException" />
        /// <exception cref="System.OverflowException" />
        void SetValue(object? value);
    }
}
