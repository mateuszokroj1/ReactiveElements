using ReactiveElements.Generators;
using ReactiveElements.Helpers;

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ReactiveElements
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get <see cref="Property{TProperty}"/> instance generated from property in <paramref name="model"/> that implements <see cref="INotifyPropertyChanged"/>
        /// </summary>
        /// <typeparam name="TModel">Model that implements <see cref="INotifyPropertyChanged"/></typeparam>
        /// <typeparam name="TProperty">Selected property type</typeparam>
        /// <param name="model">Model that implements <see cref="INotifyPropertyChanged"/></param>
        /// <param name="propertySelector">Expression with selected property from current model</param>
        /// <returns>Generated <see cref="Property{TProperty}"/>.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public static Property<TProperty> GetReactiveProperty<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> propertySelectionExpression)
            where TModel : INotifyPropertyChanged
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (propertySelectionExpression is null)
                throw new ArgumentNullException(nameof(propertySelectionExpression));

            var propertyInfo = PropertyInfoExtractor.Extract(model, propertySelectionExpression);

            return ReactivePropertyGenerator.GenerateReactivePropertyFromPropertyChangedEventModel<TModel, TProperty>(model, propertyInfo);
        }

        public static IReadonlyProperty<T> ToReadonlyReactiveProperty<T>(this IObservable<T> observable)
        => new ReadonlyProperty<T>(observable);

        public static IProperty<T> ToReactiveProperty<T>(this IObservable<T> observable)
        => new Property<T>(observable);
    }
}
