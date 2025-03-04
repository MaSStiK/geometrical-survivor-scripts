using UnityEngine;
using UnityEngine.UI; // Для работы с Image

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 10;
    public int Health { get; private set; }

    public Image HealthBar; // Ссылка на саму полоску здоровья
    public Image HealthBarBackground; // Ссылка на фон полоски здоровья
    public Transform HealthBarPosition; // Позиция для отображения полоски

    private void Start()
    {
        Health = MaxHealth;

        // Инициализируем полоску здоровья
        if (HealthBar != null)
        {
            HealthBar.fillAmount = (float)Health / MaxHealth; // Устанавливаем начальное значение полоски
        }

        if (HealthBarBackground != null)
        {
            HealthBarBackground.fillAmount = 1f; // Полоска фона должна быть заполнена полностью
        }
    }

    private void FixedUpdate()
    {
        // Обновляем позицию полоски здоровья, чтобы она следовала за игроком
        if (HealthBar != null && HealthBarPosition != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(HealthBarPosition.position); 
            HealthBar.transform.position = screenPosition;  // Перемещаем полоску в мировые координаты
            HealthBarBackground.transform.position = screenPosition;  // Перемещаем фон
        }
    }

    public void TakeDamage(int amount)
    {
        Health = Mathf.Max(0, Health - amount); // Защита от отрицательного здоровья

        // Обновляем полоску здоровья
        if (HealthBar != null)
        {
            HealthBar.fillAmount = (float)Health / MaxHealth; // Обновляем ширину полоски
        }

        Debug.Log($"Игрок получил урон! Здоровье: {Health}");

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        // Логика смерти (например, рестарт сцены)
    }
}
