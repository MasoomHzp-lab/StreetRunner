using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StageGate : MonoBehaviour
{
    [SerializeField] private RunnerGameManager gameManager;

    private bool hasTriggered;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        boxCollider.isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        if (gameManager != null)
        {
            gameManager.CompleteStage();
        }
        else
        {
            Debug.LogWarning(
                "GameManager is not connected to StageGate!"
            );
        }
    }
}