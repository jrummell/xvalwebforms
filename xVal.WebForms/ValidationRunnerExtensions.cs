using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace xVal.WebForms
{
    /// <summary>
    /// Generic <see cref="IValidationRunner"/> extension methods.
    /// </summary>
    public static class ValidationRunnerExtensions
    {
        /// <summary>
        /// Validates the specified model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="runner">The runner.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> Validate<T>(this IValidationRunner runner, T model)
        {
            return runner.Validate(model);
        }

        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="runner">The runner.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> Validate<TModel, TProperty>(
            this IValidationRunner runner, Expression<Func<TModel, TProperty>> propertyExpression, TProperty propertyValue)
        {
            MemberExpression memberExpression = (MemberExpression) propertyExpression.Body;

            return runner.Validate(typeof(TModel), propertyValue, memberExpression.Member.Name);
        }

        /// <summary>
        /// Gets the validators.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="runner">The runner.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationAttribute> GetValidators<TModel, TProperty>(
            this IValidationRunner runner, Expression<Func<TModel, TProperty>> propertyExpression)
        {
            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;

            return runner.GetValidators(typeof (TProperty), memberExpression.Member.Name);
        }
    }
}