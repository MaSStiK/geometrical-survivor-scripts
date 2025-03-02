using System;
using System.Collections.Generic;

[Serializable]
public class GameLevel
{
    public List<GameSequence> game_level;
}

[Serializable]
public class GameSequence
{
    public string type; // "novel", "game", "message"
    public string novel_name; // Для типа "novel"
    public List<Enemy> enemies; // Для типа "game"
    public string message; // Для типа "message"
}

[Serializable]
public class Enemy
{
    public string type;
    public int amount;
}