using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NovelManager novelManager;
    public GameObject novelGUI;  
    public GameObject gameGUI;   
    public PlayerController playerController;
    public EnemySpawner enemySpawner;

    private Queue<GameSequence> eventQueue;  

    void Start()
    {
        LoadGameStructure();
        ProcessNextEvent();
    }

    private void LoadGameStructure()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("GameStructureData"); // Файл должен быть в "Assets/Resources/GameStructureData.json"

        if (jsonFile == null)
        {
            Debug.LogError("Не удалось загрузить GameStructureData.json!");
            return;
        }

        GameLevel gameLevel = JsonUtility.FromJson<GameLevel>(jsonFile.text);
        eventQueue = new Queue<GameSequence>(gameLevel.game_level);
    }

    private void ProcessNextEvent()
    {
        if (eventQueue == null || eventQueue.Count == 0)
        {
            Debug.Log("Игра завершена.");
            return;
        }

        GameSequence gameSequence = eventQueue.Dequeue();

        switch (gameSequence.type)
        {
            case "novel":
                StartNovel(gameSequence.novel_name, 0);
                break;

            case "game":
                StartGame(gameSequence.enemies);
                break;

            case "message":
                Debug.Log("Game message: " + gameSequence.message);
                break;

            default:
                Debug.LogError("Неизвестный тип события: " + gameSequence.type);
                ProcessNextEvent(); 
                break;
        }
    }

    private void StartNovel(string novelName, int novelStartIndex)
    {
        SetVisibleGUI("novel");
        novelManager.StartNovel(novelName, novelStartIndex);
        playerController.canMove = false;
        enemySpawner.canSpawn = false;
        Debug.Log("Старт новеллы: " + novelName);
    }

    private void StartGame(List<Enemy> enemies)
    {
        SetVisibleGUI("game");
        enemySpawner.SpawnEnemies(enemies);
        playerController.canMove = true;
        enemySpawner.canSpawn = true;
        int totalEnemies = CountEnemies(enemies);
        Debug.Log($"Старт битвы! Всего врагов в волне: {totalEnemies}");
    }

    private void SetVisibleGUI(string guiName)
    {
        switch (guiName)
        {
            case "novel":
                novelGUI.SetActive(true);
                gameGUI.SetActive(false);
                break;

            case "game":
                novelGUI.SetActive(false);
                gameGUI.SetActive(true);
                break;
        }
    }

    private int CountEnemies(List<Enemy> enemies)
    {
        int totalEnemies = 0;
        foreach (Enemy enemy in enemies)
        {
            totalEnemies += enemy.amount;
        }
        return totalEnemies;
    }

    public void NovelFinished()
    {
        Debug.Log("Новелла завершена, переключаемся на следующий этап...");
        ProcessNextEvent();
    }

    public void GameFinished()
    {
        Debug.Log("Битва завершена, переключаемся на следующий этап...");
        ProcessNextEvent();
    }
}
