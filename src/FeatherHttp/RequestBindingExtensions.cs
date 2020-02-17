using System;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Various extension methods for parsing request data from routes, query strings and and the form
    /// </summary>
    public static class RequestBindingExtensions
    {
        /// <summary>
        /// Attempts to represent the specified route parameter as the specified type.
        /// </summary>
        /// <typeparam name="TValue">The target type to convert to.</typeparam>
        /// <param name="routeValues">The route values</param>
        /// <param name="name">The route parameter name</param>
        /// <param name="value">The result of the conversion was successful.</param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        public static bool TryGet<TValue>(this RouteValueDictionary routeValues, string name, out TValue value)
        {
            return TryGet(routeValues, (d, k) => (string)d[k], name, out value);
        }

        /// <summary>
        /// Attempts to represent the specified route parameter as the specified type.
        /// </summary>
        /// <typeparam name="TValue">The target type to convert to.</typeparam>
        /// <param name="routeValues">The route values</param>
        /// <param name="name">The route parameter name</param>
        /// <returns>A tuple of the converted value and a bool that determined is the operation was successful.</returns>
        public static (TValue Value, bool Ok) Get<TValue>(this RouteValueDictionary routeValues, string name)
        {
            var ok = routeValues.TryGet(name, out TValue value);
            return (value, ok);
        }

        /// <summary>
        /// Attempts to represent the specified query string parameter as the specified type.
        /// </summary>
        /// <typeparam name="TValue">The target type to convert to.</typeparam>
        /// <param name="query">The query string values</param>
        /// <param name="name">The query string parameter name</param>
        /// <param name="value">The result of the conversion was successful.</param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        public static bool TryGet<TValue>(this IQueryCollection query, string name, out TValue value)
        {
            return TryGet(query, (d, k) => d[k], name, out value);
        }

        /// <summary>
        /// Attempts to represent the specified query string parameter as the specified type.
        /// </summary>
        /// <typeparam name="TValue">The target type to convert to.</typeparam>
        /// <param name="query">The query string values</param>
        /// <param name="name">The query string parameter name</param>
        /// <returns>A tuple of the converted value and a bool that determined is the operation was successful.</returns>
        public static (TValue Value, bool Ok) Get<TValue>(this IQueryCollection query, string name)
        {
            var ok = query.TryGet(name, out TValue value);
            return (value, ok);
        }

        /// <summary>
        /// Attempts to represent the specified form parameter as the specified type.
        /// </summary>
        /// <typeparam name="TValue">The target type to convert to.</typeparam>
        /// <param name="form">The form values</param>
        /// <param name="name">The form parameter name</param>
        /// <param name="value">The result of the conversion was successful.</param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        public static bool TryGet<TValue>(this IFormCollection form, string name, out TValue value)
        {
            return TryGet(form, (d, k) => d[k], name, out value);
        }

        /// <summary>
        /// Attempts to represent the specified form parameter as the specified type.
        /// </summary>
        /// <typeparam name="TValue">The target type to convert to.</typeparam>
        /// <param name="form">The form values</param>
        /// <param name="name">The form parameter name</param>
        /// <returns>A tuple of the converted value and a bool that determined is the operation was successful.</returns>
        public static (TValue Value, bool Ok) TryGet<TValue>(this IFormCollection form, string name)
        {
            var ok = form.TryGet(name, out TValue value);
            return (value, ok);
        }

        private static bool TryGet<TValue, TState>(TState lookup, Func<TState, string, string> get, string name, out TValue value)
        {
            if (typeof(TValue) == typeof(int))
            {
                if (int.TryParse(get(lookup, name), out var intVal))
                {
                    value = (TValue)(object)intVal;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(string))
            {
                value = (TValue)(object)get(lookup, name);
                return value != null;
            }
            else if (typeof(TValue) == typeof(Guid))
            {
                if (Guid.TryParse(get(lookup, name), out var guidVal))
                {
                    value = (TValue)(object)guidVal;
                    return true;
                }
            }
            // TODO: DateTime (which format), TimeSpan
            // Less common
            else if (typeof(TValue) == typeof(decimal))
            {
                if (decimal.TryParse(get(lookup, name), out var decimalVal))
                {
                    value = (TValue)(object)decimalVal;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(long))
            {
                if (long.TryParse(get(lookup, name), out var longVal))
                {
                    value = (TValue)(object)longVal;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(short))
            {
                if (short.TryParse(get(lookup, name), out var shortVal))
                {
                    value = (TValue)(object)shortVal;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(byte))
            {
                if (byte.TryParse(get(lookup, name), out var byteVal))
                {
                    value = (TValue)(object)byteVal;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
