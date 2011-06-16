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
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResults, true);

            return validationResults;
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

            object model = Activator.CreateInstance(modelType);

            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue,
                                          new ValidationContext(model, null, null) {MemberName = propertyName},
                                          validationResults);
            return validationResults;
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

            // check for a validation "buddy type"
            MetadataTypeAttribute metadataTypeAttribute = 
                TypeDescriptor.GetAttributes(modelType).OfType<MetadataTypeAttribute>().SingleOrDefault();
            if (metadataTypeAttribute != null)
            {
                modelType = metadataTypeAttribute.MetadataClassType;
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