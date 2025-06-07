/*using UnityEngine;

public class ChildCollisionDetector : MonoBehaviour
{
    private aiController parentController;

    void Start()
    {
        // Get the aiController from the parent
        parentController = GetComponentInParent<aiController>();
        if (parentController == null)
        {
            Debug.LogError("No aiController found in parent!");
        }
        else
        {
            Debug.Log("ChildCollisionDetector initialized successfully");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentController != null)
        {
            parentController.ChildTriggered(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (parentController != null)
        {
            parentController.ChildTriggerExit(other);
        }
    }
} */