using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб врага
    public float spawnRadius = 1f; // Радиус за пределами экрана, где появляются враги
    public float spawnInterval = 2f; // Интервал спавна

    public bool canSpawn = false; // Можно ли спавнить врагов

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (canSpawn && !IsInvoking(nameof(SpawnEnemy)))
        {
            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        }
        else if (!canSpawn)
        {
            CancelInvoke(nameof(SpawnEnemy));
        }
    }

    void SpawnEnemy()
    {
        if (!canSpawn) return; // Проверка перед спавном

        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        int side = Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

        switch (side)
        {
            case 0: // Лево
                spawnPos = new Vector3(screenBottomLeft.x - spawnRadius, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 1: // Право
                spawnPos = new Vector3(screenTopRight.x + spawnRadius, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 2: // Низ
                spawnPos = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - spawnRadius, 0);
                break;
            case 3: // Верх
                spawnPos = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + spawnRadius, 0);
                break;
        }

        return spawnPos;
    }
}
