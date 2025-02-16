using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float cameraSmoothSpeed = 3f;

    public float lookAheadDistance = 2.5f; // Насколько вперед смещать камеру
    public float lookAheadSmoothSpeed = 5f; // Скорость интерполяции взгляда
    public bool DrawGizmos = false;

    private Vector3 offset;
    private Vector3 lastTargetPosition;
    private Vector3 lookAheadVector;

    private PlayerController playerController;

    private void Start() {
        if (target == null) return;

        offset = transform.position - target.position;
        lastTargetPosition = target.position;

        // Получаем PlayerController, если он есть на target
        playerController = target.GetComponent<PlayerController>();
    }

    private void Update() {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;

        // Проверяем, есть ли PlayerController и двигается ли игрок
        bool isMoving = playerController != null && playerController.IsMoving;

        if (isMoving) {
            // Вычисляем скорость движения персонажа
            Vector3 velocity = (target.position - lastTargetPosition) / Time.deltaTime;
            lastTargetPosition = target.position;

            // Смещаем точку слежения в сторону движения
            Vector3 targetLookAhead = velocity.normalized * lookAheadDistance;
            lookAheadVector = Vector3.Lerp(lookAheadVector, targetLookAhead, lookAheadSmoothSpeed * Time.deltaTime);
        } else {
            lookAheadVector = Vector3.zero; // Мгновенно обнуляем lookAheadVector, чтобы камера сразу центрировалась
        }

        // Добавляем опережение к целевой позиции камеры
        targetPos = target.position + offset + lookAheadVector;

        // Плавно перемещаем камеру
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSmoothSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        if (DrawGizmos) {
            Gizmos.color = Color.green;
            Vector3 lineEndPosition = transform.position + lookAheadVector;
            Gizmos.DrawLine(transform.position, lineEndPosition);
            Gizmos.DrawSphere(lineEndPosition, 0.2f);
        }
    }
}
