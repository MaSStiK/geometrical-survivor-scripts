using UnityEngine;
using TMPro;

public class DisplayInfo : MonoBehaviour
{
    public TextMeshProUGUI PlayerStats; // Ссылка на текст с информацией о игроке
    public TextMeshProUGUI EnemyStats; // Ссылка на текст с информацией о врагах
    public GameObject Player; // Игрок, который задается в инспекторе

    public EnemySpawner enemySpawner; // Спавнер врагов

    private PlayerController playerController;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        playerHealth = Player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        PlayerStats.text = $@"Player
Speed: {playerController.Velocity.magnitude:F2}
Health: {playerHealth.Health}
IsMoving: {playerController.IsMoving}";


        EnemyStats.text = $@"Enemy
Total: {enemySpawner.TotalEnemiesCount}
Alive: {enemySpawner.AliveEnemiesCount}";
    }
}
