using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject playerPrefab;
    private bool isInitialized = false;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (!isInitialized)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            isInitialized = true;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && isInitialized)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            isInitialized = false;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (!IsServer) return;
        
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available!");
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not assigned!");
            return;
        }

        int spawnIndex = (int)(clientId % (ulong)spawnPoints.Count);
        Transform spawnPoint = spawnPoints[spawnIndex];

        if (spawnPoint == null)
        {
            Debug.LogError($"Spawn point at index {spawnIndex} is null!");
            return;
        }

        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkObject networkObject = player.GetComponent<NetworkObject>();
        
        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId);
        }
        else
        {
            Debug.LogError("NetworkObject component not found on player prefab!");
            Destroy(player);
        }
    }
}
