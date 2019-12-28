using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace TablePlugin
{
    public static class UtilExtensions
    {
        public static T Get<T>(this IQueryCollection query, string key)
        {
            try
            {

                StringValues result = query[key];
                return (T)Convert.ChangeType(result.ToString(), typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static bool IsBlank(this string stringContent) => string.IsNullOrEmpty(stringContent) || string.IsNullOrWhiteSpace(stringContent);

        public static bool EqualIgnoreCase(this string stringContent, string compare) => stringContent.Equals(compare, StringComparison.OrdinalIgnoreCase);
    
        public static T As<T>(this object obj)
        {
            try
            {
                return (T)Convert.ChangeType(obj.ToString(), typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    
    }
}
