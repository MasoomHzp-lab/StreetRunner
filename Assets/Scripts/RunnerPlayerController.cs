using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RunnerPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RunnerGameManager gameManager;
    [SerializeField] private Animator animator;

    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 8f;

    [Header("Lane Settings")]
    [SerializeField] private float laneCenterX = 29.42f;
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 10f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    private CharacterController characterController;

    // 0 = Left
    // 1 = Middle
    // 2 = Right
    private int currentLane = 1;

    private float verticalVelocity;
    private bool isDead;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // شروع بازی از نزدیک‌ترین لاین نسبت به موقعیت فعلی Player
        currentLane = GetClosestLane();

        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    private void Update()
    {
        if (isDead || gameManager == null || gameManager.IsGameOver)
            return;

        HandleLaneInput();
        MovePlayer();
    }

    private void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane--;
            currentLane = Mathf.Clamp(currentLane, 0, 2);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane++;
            currentLane = Mathf.Clamp(currentLane, 0, 2);
        }
    }

    private void MovePlayer()
    {
        // محاسبه خودکار X لاین مقصد
        float targetX = laneCenterX + ((currentLane - 1) * laneDistance);

        // حرکت نرم به چپ و راست
        float newX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );

        float horizontalMovement = newX - transform.position.x;

        // Gravity
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        // سرعت بازی
        float currentForwardSpeed = forwardSpeed;

        if (gameManager != null)
        {
            currentForwardSpeed = gameManager.CurrentSpeed;
        }

        Vector3 movement = new Vector3(
            horizontalMovement,
            verticalVelocity * Time.deltaTime,
            currentForwardSpeed * Time.deltaTime
        );

        characterController.Move(movement);
    }

    private int GetClosestLane()
    {
        float leftLaneX = laneCenterX - laneDistance;
        float middleLaneX = laneCenterX;
        float rightLaneX = laneCenterX + laneDistance;

        float distanceToLeft = Mathf.Abs(transform.position.x - leftLaneX);
        float distanceToMiddle = Mathf.Abs(transform.position.x - middleLaneX);
        float distanceToRight = Mathf.Abs(transform.position.x - rightLaneX);

        if (distanceToLeft < distanceToMiddle &&
            distanceToLeft < distanceToRight)
        {
            return 0;
        }

        if (distanceToRight < distanceToMiddle)
        {
            return 2;
        }

        return 1;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead)
            return;

        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
            animator.SetTrigger("Hit");
        }

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}