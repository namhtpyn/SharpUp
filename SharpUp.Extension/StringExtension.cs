using System;
using System.Collections.Generic;
using System.Text;

namespace SharpUp.Extension
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (str == null || str.Length < 2)
                return str;

            string[] words = str.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        public static string ToUpperFirst(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            char[] a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ToPascalCase(this string str) => str.ToCamelCase().ToUpperFirst();
    }
}