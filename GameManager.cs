using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject novelGUI;  // Canvas с новеллой
    public GameObject gameGUI;   // Canvas с игровым интерфейсом
    public PlayerController playerController;  // Присваиваем ссылку на PlayerController
    public EnemySpawner enemySpawner; // Спавнер врагов (или управление спавном)

    private bool isGameStarted = false;  // Проверка, началась ли игра

    void Start()
    {
        // В начале показываем только интерфейс новеллы
        if (novelGUI != null) novelGUI.SetActive(true);
        if (gameGUI != null) gameGUI.SetActive(false);
        
        // Ожидаем начала игры
        playerController.canMove = false;
        enemySpawner.canSpawn = false;
    }

    void Update()
    {
        // Переключение с новеллы на игровое состояние по нажатию клавиши (например, "N")
        if (Input.GetKeyDown(KeyCode.N) && !isGameStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        // Убираем новелл GUI и показываем Game GUI
        if (novelGUI != null) novelGUI.SetActive(false);
        if (gameGUI != null) gameGUI.SetActive(true);
        
        // Активируем игрока и спавнера врагов
        playerController.canMove = true;
        enemySpawner.canSpawn = true;
        
        
        // Начинаем игру
        isGameStarted = true;

        // Дополнительные действия, если нужно (например, музыка, анимации, etc.)
        Debug.Log("Игра началась!");
    }
}
