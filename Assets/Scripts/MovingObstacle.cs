using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float centerX = 29.42f;
    [SerializeField] private float moveRange = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private float minX;
    private float maxX;

    private void Start()
    {
        // سمت چپ جاده
        minX = centerX - moveRange;

        // سمت راست جاده
        maxX = centerX + moveRange;
    }

    private void Update()
    {
        // ساخت حرکت رفت و برگشتی بین 0 و 1
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);

        // محاسبه موقعیت X بین چپ و راست
        float newX = Mathf.Lerp(minX, maxX, t);

        // فقط X تغییر می‌کند
        transform.position = new Vector3(
            newX,
            transform.position.y,
            transform.position.z
        );
    }
}