using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Скорость врага
    public int health = 10;  // Здоровье врага
    public int attackDamage = 1;  // Урон врага

    // Событие, которое будет вызываться при смерти врага
    public event Action OnDeath; // Событие для отслеживания смерти врага

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
                playerHealth?.TakeDamage(attackDamage); // Наносим урон игроку
                lastDamageTime = Time.time; // Обновляем время последнего урона
            }
        }
    }

    // Метод, который будет вызываться при получении урона
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();  // Если здоровье <= 0, вызываем смерть врага
        }
    }

    // Метод для смерти врага
    private void Die()
    {
        // Действия, которые нужно выполнить при смерти врага
        Debug.Log("Враг погиб!");

        // Вызываем событие OnDeath
        OnDeath?.Invoke();

        // Удаляем врага
        Destroy(gameObject);
    }
}
