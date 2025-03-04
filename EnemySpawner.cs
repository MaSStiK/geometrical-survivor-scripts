using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyPrefabData> enemyPrefabsList; // Список типов врагов и их префабов
    public GameManager gameManager;
    [SerializeField] private float spawnRadius = 1f; // Радиус за пределами экрана, где появляются враги
    [SerializeField] private float spawnInterval = 1f; // Интервал спавна
    public bool CanSpawn = false;

    public int TotalEnemiesCount { get; private set; } // Общее количество врагов в волне
    public int AliveEnemiesCount { get; private set; } // Количество живых врагов
    private int spawnEnemiesCount; // Число врагов которое нужно заспавнить

    private Camera mainCamera;
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();
    private List<Enemy> enemiesQueue = new List<Enemy>(); // Очередь врагов для спавна

    void Start()
    {
        mainCamera = Camera.main;

        foreach (var data in enemyPrefabsList)
        {
            enemyPrefabs[data.type] = data.prefab;
        }
    }

    public void SpawnEnemies(List<Enemy> enemies)
    {
        if (enemies == null || enemies.Count == 0) return;

        CanSpawn = true;
        enemiesQueue = new List<Enemy>(enemies);
        spawnEnemiesCount = enemiesQueue.Sum(e => e.amount);
        TotalEnemiesCount = CountEnemies(enemies);
        AliveEnemiesCount = TotalEnemiesCount; // Изначально количество врагов равно максимальному числу врагов в волне

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

        int randomIndex = Random.Range(0, enemiesQueue.Count);
        enemiesQueue[randomIndex].amount--;

        return enemiesQueue[randomIndex].type;
    }

    private void SpawnEnemy(string enemyType)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        if (enemyPrefabs.TryGetValue(enemyType, out GameObject enemyPrefab))
        {
            GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Подписываемся на событие смерти врага
            EnemyAI enemyAI = enemyObj.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.OnDeath += HandleEnemyDeath;
            }
        }
        else
        {
            Debug.LogError($"Префаб для {enemyType} не найден!");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        int side = Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

        switch (side)
        {
            case 0: spawnPos = new Vector3(screenBottomLeft.x - spawnRadius, Random.Range(screenBottomLeft.y, screenTopRight.y), 0); break;
            case 1: spawnPos = new Vector3(screenTopRight.x + spawnRadius, Random.Range(screenBottomLeft.y, screenTopRight.y), 0); break;
            case 2: spawnPos = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - spawnRadius, 0); break;
            case 3: spawnPos = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + spawnRadius, 0); break;
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

    private int CountEnemies(List<Enemy> enemies)
    {
        int totalEnemies = 0;
        foreach (Enemy enemy in enemies)
        {
            totalEnemies += enemy.amount;
        }
        return totalEnemies;
    }
}

[System.Serializable]
public class EnemyPrefabData
{
    public string type;
    public GameObject prefab;
}
