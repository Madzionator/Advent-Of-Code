using System.Text;

namespace Advent.Helpers.Extensions
{
    public static class StringUtils
    {
        public static string ReplaceAt(this string str, int startIndex, string value, int removeLength = 1)
        {
           var sb = new StringBuilder(str)
                .Remove(startIndex, removeLength)
                .Insert(startIndex, value);

            return sb.ToString();
        }
    }
}
