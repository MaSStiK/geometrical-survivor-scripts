using UnityEngine;
using UnityEngine.UI; // Для работы с Image

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health { get; private set; }

    public Image healthBar; // Ссылка на саму полоску здоровья
    public Image healthBarBackground; // Ссылка на фон полоски здоровья
    public Transform healthBarPosition; // Позиция для отображения полоски (например, над головой игрока)

    private void Start()
    {
        health = maxHealth;

        // Инициализируем полоску здоровья
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)health / maxHealth; // Устанавливаем начальное значение полоски
        }

        if (healthBarBackground != null)
        {
            // Убедимся, что фон полоски отображается правильно
            healthBarBackground.fillAmount = 1f; // Полоска фона должна быть заполнена полностью
        }
    }

    private void FixedUpdate()
    {
        // Обновляем позицию полоски здоровья, чтобы она следовала за игроком
        if (healthBar != null && healthBarPosition != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(healthBarPosition.position); 
            healthBar.transform.position = screenPosition;  // Перемещаем полоску в мировые координаты
            healthBarBackground.transform.position = screenPosition;  // Перемещаем фон
        }
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount); // Защита от отрицательного здоровья

        // Обновляем полоску здоровья
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)health / maxHealth; // Обновляем ширину полоски
        }

        Debug.Log("Игрок получил урон! Здоровье: " + health);

        if (health <= 0)
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
