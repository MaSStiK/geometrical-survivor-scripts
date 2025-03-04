using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float cameraSmoothSpeed = 3f; // Скорость движения камеры
    [SerializeField] private float lookAheadDistance = 2.5f; // Насколько вперед смещать камеру
    [SerializeField] private float lookAheadSmoothSpeed = 5f; // Ускорение интерполяции в сторону взгляда
    [SerializeField] private bool doDrawGizmos = false;

    private Vector3 offset;
    private Vector3 lastTargetPosition;
    private Vector3 lookAheadVector;
    private PlayerController playerController;

    private void Start() {
        if (Target == null) return;

        offset = transform.position - Target.position;
        lastTargetPosition = Target.position;

        // Получаем PlayerController, если он есть на target
        playerController = Target.GetComponent<PlayerController>();
    }

    private void Update() {
        if (Target == null) return;

        Vector3 targetPos = Target.position + offset;

        // Проверяем, есть ли PlayerController и двигается ли игрок
        bool isMoving = playerController != null && playerController.IsMoving;

        if (isMoving) {
            // Вычисляем скорость движения персонажа
            Vector3 velocity = (Target.position - lastTargetPosition) / Time.deltaTime;
            lastTargetPosition = Target.position;

            // Смещаем точку слежения в сторону движения
            Vector3 targetLookAhead = velocity.normalized * lookAheadDistance;
            lookAheadVector = Vector3.Lerp(lookAheadVector, targetLookAhead, lookAheadSmoothSpeed * Time.deltaTime);
        } else {
            lookAheadVector = Vector3.zero; // Мгновенно обнуляем lookAheadVector, чтобы камера сразу центрировалась
        }

        // Добавляем опережение к целевой позиции камеры
        targetPos = Target.position + offset + lookAheadVector;

        // Плавно перемещаем камеру
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSmoothSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        if (doDrawGizmos) {
            Gizmos.color = Color.green;
            Vector3 lineEndPosition = transform.position + lookAheadVector;
            Gizmos.DrawLine(transform.position, lineEndPosition);
            Gizmos.DrawSphere(lineEndPosition, 0.2f);
        }
    }
}
