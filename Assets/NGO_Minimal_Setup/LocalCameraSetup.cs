using UnityEngine;
using Unity.Netcode;

public class LocalCameraSetup : NetworkBehaviour
{
    public GameObject cameraObject;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner && cameraObject != null)
        {
            cameraObject.SetActive(false); // or destroy it
        }
        else
        {
            cameraObject.transform.SetParent(null); // Make it scene-local
        }
    }
}
