using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    public event Action OnDeath; // Событие для отслеживания смерти врага

    public int EnemySpeed { get; private set; }
    public int EnemyHealth { get; private set; }
    public int EnemyArmor { get; private set; }
    public int EnemyDamage { get; private set; }

    private Transform Player;
    private PlayerHealth playerHealth;
    private static float lastDamageTime = 0f; // Время последнего урона
    private static float damageCooldown = 1f; // Задержка перед нанесением урона

    // Для предотвращения наезда на других врагов
    private float avoidanceRadius = 1f; // Радиус для избегания других врагов
    private float minimumDistance = 1.5f; // Минимальное расстояние до других врагов

    private Rigidbody2D rb;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            Player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>(); // Получаем компонент здоровья игрока
        }

        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
    }

    public void SetStats(int speed, int health, int armor, int damage)
    {
        this.EnemySpeed = speed;
        this.EnemyHealth = health;
        this.EnemyArmor = armor;
        this.EnemyDamage = damage;
    }

    void Update()
    {
        if (Player != null)
        {
            // Избегаем других врагов
            AvoidOtherEnemies();

            // Двигаемся к игроку
            Vector2 direction = (Player.position - transform.position).normalized; // Направление к игроку
            rb.linearVelocity = direction * EnemySpeed; // Перемещаем через Rigidbody2D
        }
    }

    // Метод для предотвращения наезда на других врагов
    private void AvoidOtherEnemies()
    {
        // Получаем все коллайдеры вокруг врага в радиусе avoidanceRadius
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, LayerMask.GetMask("Enemy"));

        foreach (var enemy in nearbyEnemies)
        {
            // Если это не тот же самый враг
            if (enemy != this.GetComponent<Collider2D>())
            {
                // Вычисляем дистанцию до другого врага
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                // Если враг слишком близко, избегаем его
                if (distance < minimumDistance)
                {
                    // Вычисляем направление избегания
                    Vector2 direction = (transform.position - enemy.transform.position).normalized;
                    rb.linearVelocity = direction * EnemySpeed; // Перемещаем через Rigidbody2D
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Убедимся, что прошло достаточно времени, чтобы нанести урон
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                // Получаем компонент PlayerHealth у игрока и наносим урон
                playerHealth?.TakeDamage(EnemyDamage);
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
        OnDeath?.Invoke(); // Вызываем событие OnDeath
        Destroy(gameObject); // Удаляем врага
    }
}
