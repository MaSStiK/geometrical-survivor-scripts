using System.Collections.Generic;

[System.Serializable]
public class Novel
{
    public string name;
    public string text;
    public string font;
    public string spriteName;
    public string windowColor;
}

[System.Serializable]
public class NovelData
{
    public List<Novel> novel;
}
