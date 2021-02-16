using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetExtract
{
    internal static class Extensions
    {
        internal static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray()).Trim();
        }
        internal static byte[] ToBytes(this Stream stream)
        {
            if (stream is MemoryStream stream1)
                return stream1.ToArray();

            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        internal static IEnumerable<T> TakeLast<T>(this IList<T> list, int n)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count - n < 0)
            {
                n = list.Count;
            }

            for (var i = list.Count - n; i < list.Count; i++)
            {
                yield return list[i];
            }
        }
        internal static bool IsMatch(this string @this,string regexPattern)
        {
            var regex = new Regex(regexPattern);
            return regex.IsMatch(@this);
        }
    }
}