using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

using ReactiveElements.Generators;

namespace ReactiveElements
{
    public static class INotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Get <see cref="ReactiveProperty{Tproperty}"/> generated from property in <paramref name="model"/> that implements <see cref="INotifyPropertyChanged"/>
        /// </summary>
        /// <typeparam name="TModel">Model that implements <see cref="INotifyPropertyChanged"/></typeparam>
        /// <typeparam name="TProperty">Selected property type</typeparam>
        /// <param name="model">Model that implements <see cref="INotifyPropertyChanged"/></param>
        /// <param name="propertySelector">Expression with selected property from current model</param>
        /// <returns>Generated <see cref="ReactiveProperty{Tproperty}"/></returns>
        public static ReactiveProperty<TProperty> GetReactiveProperty<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> propertySelectionExpression)
            where TModel : INotifyPropertyChanged
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (propertySelectionExpression == null)
                throw new ArgumentNullException(nameof(propertySelectionExpression));

            // Getting property name from expression

            string propertyName = null;

            if (propertySelectionExpression is MemberExpression && ((UnaryExpression)propertySelectionExpression.Body).Operand is MemberExpression expression)
            {
                if (expression.Member == null || string.IsNullOrEmpty(expression.Member.Name))
                    throw new ArgumentException("Cannot read name of selected property.");

                propertyName = expression.Member.Name;
            }
            else
                throw new ArgumentException($"{nameof(propertySelectionExpression)} is not a MemberExpression. Use expression, like: (model) => model.Property.");

            // Reading property info

            PropertyInfo propertyInfo = null;

            try
            {
                propertyInfo = model.GetType().GetProperty(propertyName);
            }
            catch(AmbiguousMatchException exc)
            {
                throw new ArgumentException("Not found selected property in current model.", exc);
            }

            // Generating IObservable from 

            var observable = ObservableGenerator.GenerateObservableFromPropertyChangedEventModel<TModel, TProperty>(model, propertyInfo);

            return new ReactiveProperty<TProperty>((TProperty)propertyInfo.GetValue(model), observable.);
        }
    }
}
