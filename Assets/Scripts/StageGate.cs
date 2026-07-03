using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class StageGate : MonoBehaviour
{
    public RunnerGameManager gameManager;
    public int stageNumber = 1;

    private bool triggered;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        RunnerPlayerController player = other.GetComponent<RunnerPlayerController>();

        if (player != null)
        {
            triggered = true;

            if (gameManager != null)
                gameManager.CompleteStage(stageNumber);
        }
    }
}