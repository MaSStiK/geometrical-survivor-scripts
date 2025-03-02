using UnityEngine;
using TMPro;

public class DisplayInfo : MonoBehaviour
{
    public TextMeshProUGUI playerStats; // Ссылка на текст с информацией о игроке
    public TextMeshProUGUI enemyStats; // Ссылка на текст с информацией о врагах
    public GameObject player; // Игрок, который задается в инспекторе
    // public EnemySpawner enemySpawner; // Спавнер врагов

    private PlayerController playerController;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerController == null || playerHealth == null) return;

        playerStats.text = $@"Player
Speed: {playerController.velocity.magnitude:F2}
Health: {playerHealth.health}
IsMoving: {playerController.IsMoving}";
    }
}
