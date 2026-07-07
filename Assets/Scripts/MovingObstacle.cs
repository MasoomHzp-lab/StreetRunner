using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private float startX;


    private void Start()
    {
        // محل اولیه مانع را ذخیره می‌کنیم
        startX = transform.position.x;
    }


    private void Update()
    {
        // حرکت رفت و برگشتی روی محور X
        float offset = Mathf.PingPong(
            Time.time * moveSpeed,
            moveDistance * 2f
        ) - moveDistance;


        Vector3 newPosition = transform.position;

        newPosition.x = startX + offset;

        transform.position = newPosition;
    }
}