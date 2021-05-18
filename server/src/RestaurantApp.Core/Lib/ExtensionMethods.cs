using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestaurantApp.Core.Factory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RestaurantApp.Core.Model;

namespace RestaurantApp.Core.Lib
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Creates a new type of error model depending on the errors in the the model state.
        /// </summary>
        /// <param name="modelState">A model state dictionary where each key (property name) has a list of error messages related to that key</param>
        /// <param name="dynamicFactory">A dynamic factory for creating new types</param>
        /// <returns>A instance of the new created type</returns>
        public static object GetErrors(this ModelStateDictionary modelState, DynamicTypeFactory dynamicFactory)
        {
            List<DynamicProperty> dynamicProperties = new List<DynamicProperty>();

            foreach (var key in modelState.Keys)
            {
                if(modelState[key].Errors.Count > 0)
                {
                    dynamicProperties.Add(new DynamicProperty
                    {
                        DisplayName = key.ToCamelCase(),
                        PropertyName = key.ToCamelCase(),
                        SystemTypeName = modelState[key].Errors.Count > 1 ? typeof(List<string>).ToString() : typeof(string).ToString()
                    });
                }
            }

            var errorType = dynamicFactory.CreateNewTypeWithDynamicProperties(typeof(ErrorModel), dynamicProperties);
            var errorObject = Activator.CreateInstance(errorType);

            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count > 0)
                {
                    dynamic value;
                    List<string> errors = new List<string>();


                    foreach (var error in modelState[key].Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    if (errors.Count > 1)
                    {
                        value = errors;
                    }
                    else
                    {
                        value = errors[0];
                    }

                    errorType.GetProperty($"{key.ToCamelCase()}")
                             .SetValue(errorObject, value);
                }
            }
            return errorObject;
        }

        /// <summary>
        /// Creates a new type of error model depending on the errors in the dictionary.
        /// </summary>
        /// <param name="dict">A dictionary where each key (property name) has a list of error messages related to that key</param>
        /// <param name="dynamicFactory">A dynamic factory for creating new types</param>
        /// <returns>A instance of the new created type</returns>
        public static object GetModelError(this Dictionary<string,List<string>> dict, DynamicTypeFactory dynamicFactory)
        {
            List<DynamicProperty> dynamicProperties = new List<DynamicProperty>();

            foreach (var key in dict.Keys)
            {
                dynamicProperties.Add(new DynamicProperty
                {
                    DisplayName = key.ToCamelCase(),
                    PropertyName = key.ToCamelCase(),
                    SystemTypeName = dict[key].Count > 1 ? typeof(List<string>).ToString() : typeof(string).ToString()
                });
            }

            var errorType = dynamicFactory.CreateNewTypeWithDynamicProperties(typeof(ErrorModel), dynamicProperties);
            var errorObject = Activator.CreateInstance(errorType);

            foreach (var key in dict.Keys)
            {
                dynamic value;
                if (dict[key].Count > 1)
                {
                    value = dict[key];
                }
                else
                {
                    value = dict[key][0];
                }

                errorType.GetProperty($"{key.ToCamelCase()}")
                         .SetValue(errorObject, value);
            }
            return errorObject;
        }

        /// <summary>
        /// Transforming the property name to camel case convention.
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>A name which satisfies a camel case convention</returns>
        public static string ToCamelCase(this string propertyName)
        {
            if (propertyName != null)
            {
                return string.Join(".", propertyName.Split('.').Select(n => char.ToLower(n[0], CultureInfo.InvariantCulture) + n[1..]));
            }
            return propertyName;
        }

        /// <summary>
        ///  Get registred dependecy injected type from service container.
        /// </summary>
        /// <param name="serviceProvider">Service provider for registration</param>
        /// <returns>Returnes dependecy injected type</returns>
        public static T Resolve<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
