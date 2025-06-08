using UnityEngine;
using Unity.Netcode;

public class AICarSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject aiCarPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] waypoints;
    
    private void Start()
    {
        if (IsServer)
        {
            SpawnAICar();
            Debug.Log("Spawned AI car");
        }
    }

    private void SpawnAICar()
    {
        if (spawnPoints.Length == 0 || waypoints.Length == 0)
        {
            Debug.LogError("Spawn points or waypoints not assigned!");
            return;
        }

        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn the AI car
        GameObject aiCar = Instantiate(aiCarPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Get the AI controller component
        aiController aiController = aiCar.GetComponent<aiController>();
        if (aiController != null)
        {
            // Assign waypoints to the AI controller
            aiController.SetWaypoints(waypoints);
        }
        else
        {
            Debug.LogError("AI Controller component not found on the AI car prefab!");
        }
    }
} 