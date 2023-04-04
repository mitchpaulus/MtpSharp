using System.Text;
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

    public static int NaturalCompare(this string a, string b, StringComparer? comparer = null)
    {
        int i = 0; // index of a
        int j = 0; // index of b
        StringBuilder currentA = new(a.Length);
        StringBuilder currentB = new(b.Length);

        while (i < a.Length || j < b.Length)
        {
            // Read first chunk of digits or non-digits for a
            bool isDigitA = false;
            currentA.Clear();
            if (i < a.Length) {
                if (char.IsDigit(a[i])) {
                    while (i < a.Length && char.IsDigit(a[i])) {
                        currentA.Append(a[i]);
                        i++;
                    }
                    isDigitA = true;
                }
                else {
                    while (i < a.Length && !char.IsDigit(a[i])) {
                        currentA.Append(a[i]);
                        i++;
                    }
                }
            }

            currentB.Clear();
            bool isDigitB = false;
            if (j < b.Length) {
                if (char.IsDigit(b[j])) {
                    while (j < b.Length && char.IsDigit(b[j])) {
                        currentB.Append(b[j]);
                        j++;
                    }
                    isDigitB = true;
                }
                else {
                    while (j < b.Length && !char.IsDigit(b[j])) {
                        currentB.Append(b[j]);
                        j++;
                    }
                }
            }

            // Compare the chunks
            if (isDigitA && isDigitB) {
                int intA = int.Parse(currentA.ToString());
                int intB = int.Parse(currentB.ToString());
                if (intA != intB) {
                    return intA.CompareTo(intB);
                }
            }
            else if (isDigitA && !isDigitB) {
                return -1;
            }
            else if (!isDigitA && isDigitB) {
                return 1;
            }
            else {
                int result = (comparer ?? StringComparer.OrdinalIgnoreCase).Compare(currentA.ToString(), currentB.ToString());
                if (result != 0) {
                    return result;
                }
            }
        }

        return 0;
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> list, int startIndex = 0) => list.Select(item => (item, startIndex++));
}
