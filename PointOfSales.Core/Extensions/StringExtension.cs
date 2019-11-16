using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.Core.Extensions
{
    public static class StringExtension
    {
        public static bool IsBlank(this string stringContent) => string.IsNullOrEmpty(stringContent) || string.IsNullOrWhiteSpace(stringContent);

        public static bool EqualIgnoreCase(this string stringContent, string compare) => stringContent.Equals(compare, StringComparison.OrdinalIgnoreCase);

    }
}
