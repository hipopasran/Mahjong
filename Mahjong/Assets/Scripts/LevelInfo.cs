using System.Text.RegularExpressions;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct DataForBoard
{
    public DataForBoard(int x, int y, string layoutString)
    {
        this.x = x;
        this.y = y;
        this.layoutString = layoutString;
    }

    public int x;
    public int y;
    public string layoutString;
}

public class LevelInfo : MonoBehaviour
{
    private string levelName;
    [SerializeField] private TextAsset levelLayout;

    public bool IsPlayed => PlayerPrefs.HasKey(LevelName);
    public string LevelName => GetLevelName();
    public DataForBoard Data => GenerateLevelData();

    private string GetLevelName()
    {
        if(string.IsNullOrEmpty(levelName))
        {
            levelName = GetLevelNameFromFileName();
        }

        return levelName;
    }

    private int GetYAxis()
    {
        return levelLayout.text.Split('\n').Length;
    }

    private int GetXAxis(int y, int count)
    {
        return count / y;
    }

    private string GetLevelNameFromFileName()
    {
        var start = levelLayout.name.IndexOf("[") + 1;
        var end = levelLayout.name.IndexOf("]");
        var result = levelLayout.name.Substring(start, end - start);

        if (string.IsNullOrEmpty(result))
        {
            return levelLayout.name;
        }
        else
        {
            return result;
        }
    }

    private string GetClearLayoutText()
    {
        var pattern = @"[^\w\\xX0]";
        return Regex.Replace(levelLayout.text, pattern, string.Empty);
    }

    private DataForBoard GenerateLevelData()
    {
        var y = GetYAxis();
        var layoutText = GetClearLayoutText();
        var x = GetXAxis(y, layoutText.Length);
        return new DataForBoard(x, y, layoutText);
    }
}
