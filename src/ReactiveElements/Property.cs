using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveElements
{
    /// <summary>
    /// 
    /// </summary>
    [Bindable(true, BindingDirection.TwoWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value)), DefaultMember(nameof(Value))]
    public sealed class Property<T> : ReadonlyProperty<T>, IProperty<T>
    {
        #region Constructors

        public Property() : this(default(T)!) { }

        public Property(T? value) : base(value) { }

        public Property(IObservable<T> observableSource) : base(observableSource) { }

        #endregion

        #region Properties

        [Bindable(true, BindingDirection.TwoWay)]
        public new T Value
        {
            get => GetValue()!;
            set => SetValue(value);
        }

        #endregion

        #region Methods

        public new void SetValue(T? value) => base.SetValue(value);

        /// <exception cref="ArgumentException" />
        public new void SetValue(object value) => base.SetValue(value);

        public static Property<T> FromBindableModel<TModel>(TModel model, Expression<Func<TModel, T>> propertySelectionExpression)
            where TModel : INotifyPropertyChanged =>
            model == null ? throw new ArgumentNullException(nameof(model)) : model.GetReactiveProperty(propertySelectionExpression);

        #endregion
    }
}
