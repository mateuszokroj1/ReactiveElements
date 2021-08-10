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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            var observable = System.Reactive.Linq.Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
            (
                handler => model.PropertyChanged += handler,
                handler => model.PropertyChanged -= handler
            )
            .Where(args => args?.EventArgs.PropertyName == propertyInfo.Name)
            .Select(args => (TProperty)(dynamic)propertyInfo.GetValue(model));

            return new Property<TProperty>(observable);
        }
    }
}
