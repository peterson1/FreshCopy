using System;

namespace CommonTools.Lib.ns11.StringTools
{
    public static class StringExtensions
    {
        public static string Between(this string fullText,
                    string firstString, string lastString,
                    bool seekLastStringFromEnd = false)
        {
            if (fullText.IsBlank()) return string.Empty;

            int pos1 = fullText.IndexOf(firstString) + firstString.Length;
            if (pos1 == -1) return fullText;

            int pos2 = seekLastStringFromEnd ?
                fullText.LastIndexOf(lastString)
                : fullText.IndexOf(lastString, pos1);
            if (pos2 == -1 || pos2 <= pos1) return fullText;

            return fullText.Substring(pos1, pos2 - pos1);
        }


        public static bool IsBlank(this string text)
        {
            if (text == null) return true;
            return string.IsNullOrWhiteSpace(text);
        }



        public static bool HasText(this string lookInHere, string findThis)
        {
            var allLength = lookInHere.Length;
            var subLength = findThis.Length;
            var difLength = lookInHere.Replace(findThis, String.Empty).Length;
            return (allLength - difLength) / subLength > 0;
        }
    }
}
