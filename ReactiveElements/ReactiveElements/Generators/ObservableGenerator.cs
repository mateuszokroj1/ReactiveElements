using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;

namespace ReactiveElements.Generators
{
    public static class ObservableGenerator
    {
        public static IObservable<TProperty> GenerateObservableFromPropertyChangedEventModel<TModel, TProperty>(TModel model, PropertyInfo propertyInfo)
            where TModel : INotifyPropertyChanged
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            return System.Reactive.Linq.Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
            (
                handler => model.PropertyChanged += handler,
                handler => model.PropertyChanged -= handler
            )
            .ObserveOn(SynchronizationContext.Current)
            .Where(args => args?.EventArgs?.PropertyName == propertyInfo.Name)
            .Select(args => (TProperty)propertyInfo.GetValue(model));
        }
    }
}
