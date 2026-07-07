using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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
    [SerializeField] private float laneChangeSpeed = 12f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -25f;

    private CharacterController characterController;

    private int currentLane = 1;

    private float verticalVelocity;

    private bool hasStarted = false;
    private bool isDead = false;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    private void Start()
    {
        currentLane = GetClosestLane();

        hasStarted = false;
        isDead = false;

        // توقف انیمیشن Run روی فریم اول
        if (animator != null)
        {
            animator.Play("Run", 0, 0f);
            animator.Update(0f);
            animator.speed = 0f;
        }

        Debug.Log("GAME READY - PRESS SPACE");
    }


    private void Update()
    {
        if (isDead)
            return;


        // تا قبل از Space بازی شروع نشود
        if (!hasStarted)
        {
            if (StartButtonPressed())
            {
                StartRunning();
            }

            return;
        }


        // بعد از باخت یا برد حرکت نکن
        if (gameManager != null && gameManager.IsGameOver)
            return;


        HandleLaneInput();
        MovePlayer();
    }


    private bool StartButtonPressed()
    {
        // New Input System
#if ENABLE_INPUT_SYSTEM

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            return true;
        }

#endif


        // Old Input System
#if ENABLE_LEGACY_INPUT_MANAGER

        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

#endif

        return false;
    }


    private void StartRunning()
    {
        hasStarted = true;

        Debug.Log("RUN STARTED!");

        if (animator != null)
        {
            // شروع انیمیشن واقعی
            animator.speed = 1f;
        }
    }


    private void HandleLaneInput()
    {
        bool moveLeft = false;
        bool moveRight = false;


        // New Input System
#if ENABLE_INPUT_SYSTEM

        if (Keyboard.current != null)
        {
            moveLeft =
                Keyboard.current.aKey.wasPressedThisFrame ||
                Keyboard.current.leftArrowKey.wasPressedThisFrame;

            moveRight =
                Keyboard.current.dKey.wasPressedThisFrame ||
                Keyboard.current.rightArrowKey.wasPressedThisFrame;
        }

#endif


        // Old Input System
#if ENABLE_LEGACY_INPUT_MANAGER

        moveLeft |=
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow);

        moveRight |=
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow);

#endif


        if (moveLeft)
        {
            currentLane--;

            currentLane = Mathf.Clamp(
                currentLane,
                0,
                2
            );
        }


        if (moveRight)
        {
            currentLane++;

            currentLane = Mathf.Clamp(
                currentLane,
                0,
                2
            );
        }
    }


    private void MovePlayer()
    {
        // مقصد لاین
        float targetX =
            laneCenterX +
            ((currentLane - 1) * laneDistance);


        // حرکت نرم چپ و راست
        float newX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );


        float horizontalMovement =
            newX - transform.position.x;


        // Gravity
        if (characterController.isGrounded &&
            verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }


        verticalVelocity +=
            gravity * Time.deltaTime;


        // سرعت حرکت جلو
        float currentForwardSpeed = forwardSpeed;


        if (gameManager != null &&
            gameManager.CurrentSpeed > 0f)
        {
            currentForwardSpeed =
                gameManager.CurrentSpeed;
        }


        // حرکت واقعی Player
        Vector3 movement = new Vector3(
            horizontalMovement,
            verticalVelocity * Time.deltaTime,
            currentForwardSpeed * Time.deltaTime
        );


        characterController.Move(movement);
    }


    private int GetClosestLane()
    {
        float leftLaneX =
            laneCenterX - laneDistance;

        float middleLaneX =
            laneCenterX;

        float rightLaneX =
            laneCenterX + laneDistance;


        float distanceToLeft =
            Mathf.Abs(
                transform.position.x - leftLaneX
            );

        float distanceToMiddle =
            Mathf.Abs(
                transform.position.x - middleLaneX
            );

        float distanceToRight =
            Mathf.Abs(
                transform.position.x - rightLaneX
            );


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


    private void OnControllerColliderHit(
        ControllerColliderHit hit)
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
        if (isDead)
            return;


        isDead = true;


        if (animator != null)
        {
            animator.speed = 1f;
            animator.SetTrigger("Hit");
        }


        if (gameManager != null)
        {
            gameManager.PlayHitSound();
            gameManager.GameOver();
        }
    }
}