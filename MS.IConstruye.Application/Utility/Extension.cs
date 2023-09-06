using System;
using System.Linq;
using System.Text;

namespace MS.IConstruye.Application
{
    public static class Extension
    {
        public static string RemoveChars(this string input, params char[] chars)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (!chars.Contains(input[i]))
                    sb.Append(input[i]);
            }
            return sb.ToString();
        }
        public static string ToUTF8(this string text) => Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));

        public static object GetPropValue(object src, string propName) => src.GetType().GetProperty(propName).GetValue(src, null);
    }
}
