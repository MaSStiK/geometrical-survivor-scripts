using UnityEngine;
using System.Linq;

public class AutoShooting : MonoBehaviour
{
    public GameObject BulletPrefab; // Префаб пули
    public float bulletSpeed = 20f; // Скорость пули
    public float fireRate = 1f; // Интервал выстрела
    public float detectionRadius = 6f; // Радиус обнаружения врагов

    private float nextFireTime = 0f;

    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot(nearestEnemy.transform);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies
            .Where(e => Vector2.Distance(transform.position, e.transform.position) <= detectionRadius)
            .OrderBy(e => Vector2.Distance(transform.position, e.transform.position))
            .FirstOrDefault();
    }

    void Shoot(Transform enemy)
    {
        Vector2 direction = (enemy.position - transform.position).normalized;

        GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}
