using UnityEngine;
using UnityEditor; // Для Handles (гизмо)

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float deceleration = 40;
    public bool IsMoving { get; private set; }

    private Vector2 velocity;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем Rigidbody2D
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(moveX, moveY);

        if (input.magnitude > 0)
        {
            input.Normalize();
            velocity = input * speed;
        }
        else
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        IsMoving = velocity.magnitude > 0.1f;
    }

    void FixedUpdate() // Используем FixedUpdate для работы с физикой
    {
        rb.linearVelocity = velocity; // Двигаем игрока через Rigidbody
    }

    private void OnDrawGizmos()
    {
        Color textColor = IsMoving ? Color.green : Color.red;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = textColor;
        style.fontSize = 12;
        style.alignment = TextAnchor.MiddleCenter;

        Vector3 textPosition = transform.position + Vector3.right * 2.5f;

        Handles.Label(textPosition, "isMoving", style);
    }
}
