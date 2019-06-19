using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace RD.CodeGenerator
{
    public static class Extentions
    {
        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");
            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static string ToCamelCase(this string input, char separator = '_')
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");

            string[] values = input.Split(new char[] { separator });

            StringBuilder sb = new StringBuilder();
            foreach (string str in values)
            {
                sb.Append(str.First().ToString().ToUpper() + str.Substring(1).ToLower());
            }

            return sb.ToString();

        }

        public static string ToCamelCaseWithSeparator(this string input, char separator = '_')
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("input Is Null Or Empty");

            string[] values = input.Split(new char[] { separator });

            String sb = "";

            string[] finalValues = new string[values.Length];
            int count = 0;
            foreach (string str in values)
            {
                finalValues[count] = (str.First().ToString().ToUpper() + str.Substring(1).ToLower());
                count++;
            }
            sb = string.Join(separator.ToString(), finalValues);
            return sb;

        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static void ForEach<T>(this ObservableCollection<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

    }
}
