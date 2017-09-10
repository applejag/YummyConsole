using System;
using System.Collections.Generic;

namespace YummyConsole.Helpers
{
    public static class StringHelper
    {
        public static readonly string[] Newlines = {"\n\r", "\r\n", "\r", "\n"};

        public static string[] SplitString(string text, int maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return new string[0];

            var list = new List<string>(text.Split(Newlines, StringSplitOptions.None));

            if (maxWidth > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Length > maxWidth)
                    {
                        if (list[i].Length != maxWidth + 1)
                            list.Insert(i + 1, list[i].Substring(maxWidth));
                        list[i] = list[i].Substring(0, maxWidth);
                    }
                }
            }

            return list.ToArray();
        }
    }
}