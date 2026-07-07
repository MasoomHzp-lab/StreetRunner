using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 120f;

    private bool collected = false;


    private void Update()
    {
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );
    }


    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;


        RunnerPlayerController player =
            other.GetComponent<RunnerPlayerController>();


        if (player == null)
            return;


        collected = true;


        RunnerGameManager gameManager =
            FindFirstObjectByType<RunnerGameManager>();


        if (gameManager != null)
        {
            gameManager.CollectCoin();
        }


        Destroy(gameObject);
    }
}