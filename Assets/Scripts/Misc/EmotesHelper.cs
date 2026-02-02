using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CFD.Misc
{
    public static class EmotesHelper
    {
        private static readonly string SpriteCode = @"<sprite index={0}>";

        private static readonly string ReplacementSymbol = "\uFFFD";
        
        /// <summary>
        /// Bind known tags to the text sprites map
        /// </summary>
        private static Dictionary<string, int > _unicodeEmotes = new Dictionary<string, int>()
        {
            {"satisfied", 0},
            {"intrigued", 1},
            {"neutral", 2},
            {"affirmative", 3},
            {"laughing", 4},
            {"win", 5},
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
                var tag = match.Groups[1].Value;
                unitcodes.Add(tag);
                if (_unicodeEmotes.TryGetValue(tag, out var index))
                {
                    text = text.Replace($"{{{tag}}}", string.Format(SpriteCode, index));
                }
                else
                {
                    text = text.Replace($"{{{tag}}}", string.Format(SpriteCode, ReplacementSymbol));
                }
            }

            return text;
        }
    }
}