using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health { get; private set; }

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount); // Защита от отрицательного здоровья
        Debug.Log("Игрок получил урон! Здоровье: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        // Добавь логику смерти (например, рестарт сцены)
    }
}
