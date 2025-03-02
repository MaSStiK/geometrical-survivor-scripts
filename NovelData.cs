using System;
using System.Collections.Generic;

[Serializable]
public class Novel
{
    public string name;
    public string text;
    public string font;
    public string spriteName;
    public string windowColor;
}

[Serializable]
public class NovelData
{
    public List<Novel> novel;
}