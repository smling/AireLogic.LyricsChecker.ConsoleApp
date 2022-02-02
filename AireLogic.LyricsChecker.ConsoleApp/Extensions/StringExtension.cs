using System;
using System.Collections.Generic;
using System.Text;

namespace AireLogic.LyricsChecker.ConsoleApp.Extensions
{
    public static class StringExtension
    {
        public static int WordCount(this string text) {
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
