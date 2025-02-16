using UnityEngine;
using TMPro;

public class SpeedDisplay : MonoBehaviour
{
    public TextMeshProUGUI speedText; // Ссылка на UI-текст скорости
    public TextMeshProUGUI healthText; // Ссылка на UI-текст здоровья
    public TextMeshProUGUI isMovingText; // Ссылка на UI-текст движения
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
        if (playerController != null && speedText != null)
        {
            // Обновляем текст с информацией о скорости
            speedText.text = $"Speed: {playerController.velocity.magnitude:F2}";
        }

        if (playerHealth != null && healthText != null)
        {
            // Обновляем текст с информацией о здоровье
            healthText.text = $"Health: {playerHealth.health}";
            isMovingText.color = PlayerHealth.health > 0 ? Color.green : Color.red;
        }

        if (playerController != null && isMovingText != null)
        {
            // Обновляем текст о движении
            isMovingText.text = "IsMoving";
            isMovingText.color = playerController.IsMoving ? Color.green : Color.red;
        }
    }
}
