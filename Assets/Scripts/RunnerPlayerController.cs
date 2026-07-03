using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RunnerPlayerController : MonoBehaviour
{
    [Header("References")]
    public RunnerGameManager gameManager;
    public Animator animator;

    [Header("Lane Settings")]
    public float laneWidth = 3f;
    public float laneChangeSpeed = 12f;

    [Header("Movement")]
    public float gravity = -25f;

    [Header("Collision")]
    public string obstacleTag = "Obstacle";

    private CharacterController controller;
    private int targetLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private float verticalVelocity;
    private bool isDead;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    private void Update()
    {
        if (isDead || gameManager == null || gameManager.IsGameOver)
            return;

        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveRight();
    }

    public void MoveLeft()
    {
        if (targetLane > 0)
            targetLane--;
    }

    public void MoveRight()
    {
        if (targetLane < 2)
            targetLane++;
    }

    private void MovePlayer()
    {
        float targetX = (targetLane - 1) * laneWidth;

        float newX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );

        float deltaX = newX - transform.position.x;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -1f;

        verticalVelocity += gravity * Time.deltaTime;

        float forwardSpeed = gameManager.CurrentSpeed;

        Vector3 move = new Vector3(
            deltaX,
            verticalVelocity * Time.deltaTime,
            forwardSpeed * Time.deltaTime
        );

        controller.Move(move);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead)
            return;

        if (hit.collider.CompareTag(obstacleTag))
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

        gameManager.GameOver();
    }
}