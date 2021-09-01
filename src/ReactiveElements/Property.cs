using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveElements
{
    /// <summary>
    /// Reactive property that can be used to build an interactive interface.
    /// </summary>
    [Bindable(true, BindingDirection.TwoWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value)), DefaultMember(nameof(Value))]
    public sealed class Property<T> : ReadonlyProperty<T>, IProperty<T>
    {
        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="Property{T}"/> with default value of <typeparamref name="T"/>.
        /// </summary>
        public Property() : this(default(T)!) { }

        /// <summary>
        /// Creates new instance of <see cref="Property{T}"/> with initial value.
        /// </summary>
        /// <param name="value">Initial value of property</param>
        public Property(T? value) : base(value) { }

        /// <summary>
        /// <para>Creates a new instance of <see cref="Property{T}"/>.</para>
        /// <para>Where the values are taken from the <paramref name="observableSource"/> by default, but can also be changed manually.</para>
        /// </summary>
        /// <param name="observableSource">Default source of values</param>
        public Property(IObservable<T> observableSource) : base(observableSource) { }

        #endregion

        #region Properties

        [Bindable(true, BindingDirection.TwoWay)]
        public new T? Value
        {
            get => GetValue()!;
            set => SetValue(value);
        }

        #endregion

        #region Methods

        public new void SetValue(T? value) => base.SetValue(value);

        public new void SetValue(object? value) => base.SetValue(value);

        public static Property<T> FromBindableModel<TModel>(TModel model, Expression<Func<TModel, T>> propertySelectionExpression)
            where TModel : INotifyPropertyChanged =>
            model is null ? throw new ArgumentNullException(nameof(model)) : model.GetReactiveProperty(propertySelectionExpression);

        #endregion
    }
}
