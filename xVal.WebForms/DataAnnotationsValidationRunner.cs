using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace xVal.WebForms
{
    public class DataAnnotationsValidationRunner : IValidationRunner
    {
        private static readonly object _typePropertiesLock = new object();
        private static readonly object _propertyAttributesLock = new object();

        #region IValidationRunner Members

        /// <summary>
        /// Validates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(object model)
        {
            return Validate(model, true);
        }

        /// <summary>
        /// Validates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="validateObject">if set to <c>true</c> and <paramref name="model"/> implements <see cref="IValidatableObject"/>,
        /// calls <see cref="IValidatableObject.Validate"/> in addition to running all property validation attributes.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(object model, bool validateObject)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            IEnumerable<ValidationResult> results =
                from prop in GetProperties(model.GetType())
                from attribute in GetAttributes(prop)
                where !attribute.IsValid(prop.GetValue(model))
                select new ValidationResult(attribute.FormatErrorMessage(String.Empty), new[] {prop.Name});

            if (!validateObject)
            {
                return results;
            }

            List<ValidationResult> resultList = new List<ValidationResult>(results);

            IValidatableObject validatableObject = model as IValidatableObject;
            if (validatableObject != null)
            {
                IEnumerable<ValidationResult> objectResults =
                    validatableObject.Validate(new ValidationContext(model, null, null));

                if (objectResults.Any())
                {
                    resultList.AddRange(objectResults);
                }
            }

            return resultList;
        }

        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(Type modelType, object propertyValue, string propertyName)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "propertyName");
            }

            return from property in GetProperties(modelType)
                   where property.Name == propertyName
                   from attribute in GetAttributes(property)
                   where !attribute.IsValid(propertyValue)
                   select new ValidationResult(attribute.FormatErrorMessage(String.Empty), new[] {property.Name});
        }

        /// <summary>
        /// Gets the validators.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public IEnumerable<ValidationAttribute> GetValidators(Type modelType, string propertyName)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "propertyName");
            }

            return from property in GetProperties(modelType)
                   where property.Name == propertyName
                   from attribute in GetAttributes(property)
                   select attribute;
        }

        #endregion

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private static IEnumerable<ValidationAttribute> GetAttributes(PropertyDescriptor property)
        {
            IEnumerable<ValidationAttribute> attributes;

            if (HttpContext.Current == null)
            {
                attributes = property.Attributes.OfType<ValidationAttribute>();
            }
            else
            {
                lock (_propertyAttributesLock)
                {
                    const string cacheKey = "PropertyAttributesDictionary";
                    IDictionary<PropertyDescriptor, IEnumerable<ValidationAttribute>> propertyAttributes =
                        HttpContext.Current.Cache[cacheKey] as
                        IDictionary<PropertyDescriptor, IEnumerable<ValidationAttribute>>;
                    if (propertyAttributes == null)
                    {
                        propertyAttributes = new Dictionary<PropertyDescriptor, IEnumerable<ValidationAttribute>>();
                        HttpContext.Current.Cache.Add(cacheKey, propertyAttributes, null,
                                                      DateTime.Now.AddYears(1), Cache.NoSlidingExpiration,
                                                      CacheItemPriority.Normal, null);
                    }

                    if (propertyAttributes.ContainsKey(property))
                    {
                        attributes = propertyAttributes[property];
                    }
                    else
                    {
                        attributes = property.Attributes.OfType<ValidationAttribute>();

                        propertyAttributes.Add(property, attributes);
                    }
                }
            }

            return attributes;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns></returns>
        private static IEnumerable<PropertyDescriptor> GetProperties(Type modelType)
        {
            IEnumerable<PropertyDescriptor> properties;

            if (HttpContext.Current == null)
            {
                properties = TypeDescriptor.GetProperties(modelType).Cast<PropertyDescriptor>();
            }
            else
            {
                lock (_typePropertiesLock)
                {
                    const string cacheKey = "TypePropertiesDictionary";
                    IDictionary<Type, IEnumerable<PropertyDescriptor>> typeProperties =
                        HttpContext.Current.Cache[cacheKey] as IDictionary<Type, IEnumerable<PropertyDescriptor>>;
                    if (typeProperties == null)
                    {
                        typeProperties = new Dictionary<Type, IEnumerable<PropertyDescriptor>>();
                        HttpContext.Current.Cache.Add(cacheKey, typeProperties, null,
                                                      DateTime.Now.AddYears(1), Cache.NoSlidingExpiration,
                                                      CacheItemPriority.Normal, null);
                    }

                    if (typeProperties.ContainsKey(modelType))
                    {
                        properties = typeProperties[modelType];
                    }
                    else
                    {
                        properties = TypeDescriptor.GetProperties(modelType).Cast<PropertyDescriptor>();
                    }
                }
            }

            return properties;
        }
    }
}