using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.Helpers.Extensions
{
    public static class StringUtils
    {
        public static int[] ExtractNumbers(this string str) =>
            new Regex("\\d+").Matches(str)
                .Select(x => int.Parse(x.Value))
                .ToArray();
    }
}