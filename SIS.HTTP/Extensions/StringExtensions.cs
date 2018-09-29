namespace SIS.HTTP.Extensions
{
    using System.Globalization;

    public static class StringExtensions
    {
        // TODO: Check all string for capitalize. If this method capitalize only first letter or every first letter of separate word.
        public static string Capitalize(this string input) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }
}
