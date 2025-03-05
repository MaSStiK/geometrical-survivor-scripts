using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class EnemySpawner : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject enemyPrefab; // Префаб врага, который будет клонироваться
    [SerializeField] private float spawnRadius = 1f; // Радиус за пределами экрана, где появляются враги
    [SerializeField] private float spawnInterval = 1f; // Интервал спавна
    public bool CanSpawn = false;

    public int TotalEnemiesCount { get; private set; }
    public int AliveEnemiesCount { get; private set; }
    private int spawnEnemiesCount;

    private Camera mainCamera;
    private List<EnemyWave> enemiesQueue = new List<EnemyWave>(); // Очередь врагов для спавна
    private Dictionary<string, EnemyData> enemyStats = new Dictionary<string, EnemyData>(); // Характеристики врагов

    void Start()
    {
        mainCamera = Camera.main;
        LoadEnemyStats(); // Загружаем характеристики врагов
    }

    private void LoadEnemyStats()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("EnemiesList"); // Файл EnemiesList.json должен быть в папке Resources
        // Десериализуем JSON с использованием Newtonsoft.Json
        enemyStats = JsonConvert.DeserializeObject<Dictionary<string, EnemyData>>(jsonFile.text);
    }

    public void SpawnEnemies(List<EnemyWave> enemies)
    {
        if (enemies == null || enemies.Count == 0) return;

        CanSpawn = true;
        enemiesQueue = new List<EnemyWave>(enemies);
        spawnEnemiesCount = enemiesQueue.Sum(e => e.amount);
        TotalEnemiesCount = CountEnemies(enemies);
        AliveEnemiesCount = TotalEnemiesCount;

        StartCoroutine(SpawnEnemyWave());
    }

    private IEnumerator SpawnEnemyWave()
    {
        while (spawnEnemiesCount > 0)
        {
            if (!CanSpawn) yield break;

            string randomEnemyType = GetRandomEnemyType();
            if (!string.IsNullOrEmpty(randomEnemyType))
            {
                SpawnEnemy(randomEnemyType);
                spawnEnemiesCount--;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private string GetRandomEnemyType()
    {
        if (enemiesQueue.Count == 0) return null;

        enemiesQueue = enemiesQueue.Where(e => e.amount > 0).ToList();

        if (enemiesQueue.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, enemiesQueue.Count);
        enemiesQueue[randomIndex].amount--;

        return enemiesQueue[randomIndex].type;
    }

    private void SpawnEnemy(string enemyType)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Проверяем, существует ли тип врага в enemyStats
        if (enemyStats.ContainsKey(enemyType))
        {
            // Получаем данные врага из enemyStats
            EnemyData enemyData = enemyStats[enemyType];

            // Создаем новый объект врага, используя префаб
            GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyAI enemyAI = enemyObj.GetComponent<EnemyAI>();

            // Загружаем спрайт из ресурсов
            Sprite sprite = Resources.Load<Sprite>(enemyData.spritePath);
            if (sprite != null)
            {
                // Назначаем спрайт врага
                SpriteRenderer spriteRenderer = enemyObj.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"Спрайт для врага {enemyType} не найден по пути: {enemyData.spritePath}");
            }

            // Передаем данные врага в EnemyAI
            enemyAI.SetStats(enemyData.speed, enemyData.health, enemyData.armor, enemyData.damage);
            enemyAI.OnDeath += HandleEnemyDeath;
        }
        else
        {
            Debug.LogError($"Тип врага {enemyType} не найден в enemyStats!");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        int side = UnityEngine.Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

        switch (side)
        {
            case 0: spawnPos = new Vector3(screenBottomLeft.x - spawnRadius, UnityEngine.Random.Range(screenBottomLeft.y, screenTopRight.y), 0); break;
            case 1: spawnPos = new Vector3(screenTopRight.x + spawnRadius, UnityEngine.Random.Range(screenBottomLeft.y, screenTopRight.y), 0); break;
            case 2: spawnPos = new Vector3(UnityEngine.Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - spawnRadius, 0); break;
            case 3: spawnPos = new Vector3(UnityEngine.Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + spawnRadius, 0); break;
        }

        return spawnPos;
    }

    private void HandleEnemyDeath()
    {
        AliveEnemiesCount--;

        Debug.Log($"Враг уничтожен! Осталось {AliveEnemiesCount} врагов.");

        if (AliveEnemiesCount <= 0)
        {
            Debug.Log("Все враги уничтожены! Завершаем игру.");
            gameManager.GameFinished();
        }
    }

    private int CountEnemies(List<EnemyWave> enemies)
    {
        int totalEnemies = 0;
        foreach (EnemyWave enemy in enemies)
        {
            totalEnemies += enemy.amount;
        }
        return totalEnemies;
    }
}

[Serializable]
public class EnemyData
{
    public string spritePath; // Путь к спрайту в Resources
    public int speed;
    public int health;
    public int armor;
    public int damage;
}

[Serializable]
public class EnemyWave
{
    public string type;
    public int amount;
}
