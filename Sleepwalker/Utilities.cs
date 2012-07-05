using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Sleepwalker
{
    internal static class Utilities
    {
        public static uint EpochTime()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (uint) t.TotalSeconds;
        }

        public static string[] AllInstances(string begin, string end, string src)
        {
            MatchCollection matchs =
                new Regex(Regex.Escape(begin) + "(.*?)" + Regex.Escape(end), RegexOptions.Singleline).Matches(src);
            var strArray = new string[matchs.Count];
            for (int i = 0; i < matchs.Count; i++)
            {
                strArray[i] = matchs[i].Value.Replace(begin, "").Replace(end, "");
            }
            return strArray;
        }

        public static string EncodeNonAsciiCharacters(string value)
        {
            var sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int) c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => ((char) int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
        }
    }
}