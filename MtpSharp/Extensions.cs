using System.Text.RegularExpressions;

namespace MtpSharp;

public static class StringExtensions
{
    public static IEnumerable<string> SplitLines(this string input)
    {
        using StringReader sr = new(input);
        while (sr.ReadLine() is { } line)
        {
            yield return line;
        }
    }

    // https://stackoverflow.com/a/22323356/5932184
    public static IEnumerable<T> OrderByNatural<T>(this IEnumerable<T> items, Func<T, string> selector, StringComparer? stringComparer = null)
    {
        var regex = new Regex(@"\d+", RegexOptions.Compiled);
        IEnumerable<T> enumerable = items.ToList();
        int maxDigits = enumerable
                      .SelectMany(i => regex.Matches(selector(i)).Select(digitChunk => (int?)digitChunk.Value.Length))
                      .Max() ?? 0;

        return enumerable.OrderBy(i => regex.Replace(selector(i), match => match.Value.PadLeft(maxDigits, '0')), stringComparer ?? StringComparer.OrdinalIgnoreCase);
    }

    public static IEnumerable<T> OrderByNatural<T>(this IEnumerable<T> items, StringComparer? stringComparer = null)
    {
        return items.OrderByNatural(i => i?.ToString() ?? string.Empty, stringComparer);
    }
}
