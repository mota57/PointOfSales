using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace TablePlugin
{
    public static class UtilExtension
    {
        public static T Get<T>(this IQueryCollection query, string key)
        {
            try
            {

                StringValues result = query[key];
                return (T)Convert.ChangeType(result.ToString(), typeof(T));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static bool IsBlank(this string stringContent) => string.IsNullOrEmpty(stringContent) || string.IsNullOrWhiteSpace(stringContent);

        public static bool EqualIgnoreCase(this string stringContent, string compare) => stringContent.Equals(compare, StringComparison.OrdinalIgnoreCase);

    }
}
