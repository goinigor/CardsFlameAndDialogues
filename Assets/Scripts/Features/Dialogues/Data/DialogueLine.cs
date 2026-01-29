using System;
using CFD.Misc;

[Serializable]
public class DialogueLine
{
    public string name;
    public string text;
    
    /// <summary>
    /// Get the emotion from the text (if any)
    /// </summary>
    public string GetTextWithEmotions()
    {
        return EmotesHelper.EmoteToUnicode(text);
    }
}