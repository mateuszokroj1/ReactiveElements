using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;

namespace ReactiveElements.Generators
{
    internal static class ReactivePropertyGenerator
    {
        public static Property<TProperty> GenerateReactivePropertyFromPropertyChangedEventModel<TModel, TProperty>(TModel model, PropertyInfo propertyInfo)
            where TModel : INotifyPropertyChanged
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            var observable = System.Reactive.Linq.Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
            (
                handler => model.PropertyChanged += handler,
                handler => model.PropertyChanged -= handler
            )
            .Where(args => args?.EventArgs.PropertyName == propertyInfo.Name)
            .Select(args => (TProperty)propertyInfo.GetValue(model));

            var result = new Property<TProperty>(observable);
            result.Value = (TProperty)propertyInfo.GetValue(model);
            return result;
        }
    }
}
