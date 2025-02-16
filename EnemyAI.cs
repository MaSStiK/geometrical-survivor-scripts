using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;
    private PlayerHealth playerHealth;
    private static float lastDamageTime = 0f; // Время последнего урона
    private static float damageCooldown = 1f; // Задержка перед нанесением урона

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>(); // Получаем компонент здоровья игрока
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown) // Проверяем, прошло ли достаточно времени
            {
                playerHealth?.TakeDamage(1); // Наносим 1 урон
                lastDamageTime = Time.time; // Обновляем время последнего урона
            }
        }
    }
}
