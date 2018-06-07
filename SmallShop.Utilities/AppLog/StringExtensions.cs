using System.Text.RegularExpressions;

namespace SmallShop.Utilities.Lib.AppLog
{
    internal static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchLetter(this string input)
        {
            return input.IsMatch(@"^[A-Za-z]+$");
        }

        /// <summary>
        /// pattern
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static bool IsMatch(this string input, string pattern)
        {
            return !input.IsNullOrEmpty() && Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// Indicates whether the specified string is null or an Empty string.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        private static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}