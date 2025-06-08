using UnityEngine;
using Unity.Netcode;

public class LocalCameraSetup : NetworkBehaviour
{
    public GameObject cameraObject;
    private bool isInitialized = false;

    public override void OnNetworkSpawn()
    {
        if (!isInitialized)
        {
            SetupCamera();
            isInitialized = true;
        }
    }

    private void SetupCamera()
    {
        if (cameraObject == null)
        {
            Debug.LogError("Camera object reference is missing on LocalCameraSetup!");
            return;
        }

        if (!IsOwner)
        {
            // Disable camera for non-owner clients
            cameraObject.SetActive(false);
        }
        else
        {
            // Make camera scene-local for owner
            try
            {
                cameraObject.transform.SetParent(null);
                cameraObject.SetActive(true);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error setting up camera: {e.Message}");
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (isInitialized)
        {
            // Clean up camera when network object is destroyed
            if (cameraObject != null)
            {
                cameraObject.SetActive(false);
            }
            isInitialized = false;
        }
    }
}
