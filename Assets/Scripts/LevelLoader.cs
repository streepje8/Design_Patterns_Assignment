using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public struct LoadedNote
{
    public float time;
    public bool color;

    public LoadedNote(float time, bool color)
    {
        this.time = time;
        this.color = color;
    }
}

public class LevelLoader
{
    public static List<LoadedNote> LoadLevel(string text, float noteSpeed)
    {
        List<LoadedNote> result = new List<LoadedNote>();
        foreach(string s in text.Split('\n'))
        {
            if (s.Length > 2)
            {
                string removedWhitespace = Regex.Replace(s, @"/s+", "");
                string[] splitBySlash = removedWhitespace.Split('/');
                string removedSpecialChars = Regex.Replace(splitBySlash[1], "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
                float time = float.Parse(splitBySlash[0]) - (17f/noteSpeed);
                if(time > 0.01f)
                    result.Add(new LoadedNote(time, removedSpecialChars.Equals("BLUE", StringComparison.OrdinalIgnoreCase))); //Since theres only two cases this can be simplified
            }
        }

        return result;
    }
}