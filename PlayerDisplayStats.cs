using UnityEngine;
using TMPro;

public class PlayerDisplayStats : MonoBehaviour
{
    public TextMeshProUGUI playerStatsText; // Ссылка на UI-текст скорости
    private PlayerController playerController;
    private PlayerHealth playerHealth;

    void Start()
    {
        // Получаем компоненты игрока
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>(); // Получаем компонент здоровья
    }

    void Update()
    {

        playerStatsText.text = $@"Player
Speed: {playerController.velocity.magnitude:F2}
Health: {playerHealth.health}
IsMoving: {playerController.IsMoving}";
    }
}
