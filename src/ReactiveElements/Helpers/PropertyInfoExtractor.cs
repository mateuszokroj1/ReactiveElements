using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveElements.Helpers
{
    internal static class PropertyInfoExtractor
    {
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public static PropertyInfo Extract<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> propertySelectionExpression)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            string propertyName;

            if (propertySelectionExpression is not null &&
                propertySelectionExpression.Body is UnaryExpression expr2 &&
                expr2.Operand is MemberExpression expression
            )
            {
                if (expression.Member is null || string.IsNullOrEmpty(expression.Member.Name))
                    throw new ArgumentException("Cannot read name of selected property.");

                propertyName = expression.Member.Name;
            }
            else
            {
                throw new ArgumentException($"{nameof(propertySelectionExpression)} is not a MemberExpression. Use expression, like: (model) => model.Property.");
            }

            try
            {
                return model.GetType().GetProperty(propertyName);
            }
            catch (MemberAccessException exc)
            {
                throw new ArgumentException("Not found selected property in current model.", exc);
            }
        }
    }
}
