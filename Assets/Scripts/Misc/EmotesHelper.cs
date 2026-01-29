using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CFD.Misc
{
    public static class EmotesHelper
    {
        private static Dictionary<string, string> _unicodeEmotes = new Dictionary<string, string>()
        {
            {"satisfied", "\uD83D\uDE0A"},
            {"surprised", "\uD83D\uDE32"},
            {"angry", "\uD83D\uDE20"},
            {"disappointed", "\uD83D\uDE1E"},
            {"neutral", "\uD83D\uDE10"},
            {"happy", "\uD83D\uDE00"},
            {"sad", "\uD83D\uDE15"},
            {"skeptical", "\uD83E\uDD36"},
            {"excited", "\uD83D\uDE03"},
            {"winking", "\uD83D\uDE09"},
            {"laughing", "\uD83E\uDD24"},
            {"teeth", "\uD83D\uDE02"},
            {"confused", "\uD83D\uDE1F"},
            {"kissing", "\uD83D\uDE17"},
            {"tongue", "\uD83D\uDE1C"},
            {"blush", "\uD83D\uDE0A"},
            {"smile", "\uD83D\uDE04"},
            {"heart", "\u2665"}
        };
        
        /// <summary>
        /// Extract emotion from dialogue text (e.g., "{satisfied}" from the text)
        /// </summary>
        public static string EmoteToUnicode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            List<string> unitcodes = new List<string>();
            Regex regex = new Regex(@"\{([^}]+)\}");
            var matches = regex.Matches(text);
            

            foreach (Match match in matches)
            {
                var unitcode = match.Groups[1].Value;
                unitcodes.Add(unitcode);
                if (_unicodeEmotes.TryGetValue(unitcode, out string unicode))
                {
                    text = text.Replace($"{{{unitcode}}}", unicode);
                }
                else
                {
                    text = text.Replace($"{{{unitcode}}}", string.Empty);
                }
            }

            return text;
        }
    }
}