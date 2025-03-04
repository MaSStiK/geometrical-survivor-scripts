using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    public event Action OnDeath; // Событие для отслеживания смерти врага

    public float EnemySpeed = 3f; // Дефолтная скорость врага
    public float EnemyHealth = 10f;  // Дефолтное здоровье врага
    public int EnemyDamage = 1;  // Дефолтный урон врага

    private Transform Player;
    private PlayerHealth playerHealth;
    private static float lastDamageTime = 0f; // Время последнего урона
    private static float damageCooldown = 1f; // Задержка перед нанесением урона

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            Player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>(); // Получаем компонент здоровья игрока
        }
    }

    void Update()
    {
        if (Player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, EnemySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown) // Проверяем, прошло ли достаточно времени
            {
                playerHealth?.TakeDamage(EnemyDamage); // Наносим урон игроку
                lastDamageTime = Time.time; // Обновляем время последнего урона
            }
        }
    }

    // Метод, который будет вызываться при получении урона
    public void TakeDamage(int damage)
    {
        EnemyHealth -= damage;

        if (EnemyHealth <= 0)
        {
            Die();  // Если здоровье <= 0, вызываем смерть врага
        }
    }

    // Метод для смерти врага
    private void Die()
    {
        // Вызываем событие OnDeath
        OnDeath?.Invoke();

        // Удаляем врага
        Destroy(gameObject);
    }
}
