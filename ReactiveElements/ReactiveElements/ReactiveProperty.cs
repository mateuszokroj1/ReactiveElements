using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveElements
{
    [Bindable(true, BindingDirection.TwoWay), DefaultBindingProperty(nameof(Value)), DefaultProperty(nameof(Value)), DefaultMember(nameof(Value))]
    public sealed class ReactiveProperty<T> : ReadonlyReactiveProperty<T>, IReactiveProperty<T>
    {
        #region Constructors

        public ReactiveProperty() : this(default(T)) { }

        public ReactiveProperty(T value) : base(value) { }

        public ReactiveProperty(IObservable<T> observableSource) : base(observableSource) { }

        #endregion

        #region Properties

        [Bindable(true, BindingDirection.TwoWay)]
        public new T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        #endregion

        #region Methods

        public new void SetValue(T value)
        {
            base.SetValue(value);
        }

        /// <exception cref="ArgumentException" />
        public new void SetValue(object value)
        {
            base.SetValue(value);
        }

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
