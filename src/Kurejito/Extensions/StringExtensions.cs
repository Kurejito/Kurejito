using System;

namespace Kurejito.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="String"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines if <paramref name="value"/> is null or entirely whitespace. 
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return String.IsNullOrEmpty(value) || String.IsNullOrEmpty(value.Trim());
        }
    }
}