using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

public class RacePositionTracker : NetworkBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform[] aiPlayers; // Array of AI player transforms
    
    private Dictionary<ulong, Transform> playerTransforms = new Dictionary<ulong, Transform>();
    private Dictionary<Transform, int> aiPositions = new Dictionary<Transform, int>();
    private Dictionary<ulong, int> playerPositions = new Dictionary<ulong, int>();
    private Dictionary<ulong, int> playerWaypointProgress = new Dictionary<ulong, int>();
    private Dictionary<ulong, float> playerDistanceToNextWaypoint = new Dictionary<ulong, float>();
    private Dictionary<Transform, int> aiWaypointProgress = new Dictionary<Transform, int>();
    private Dictionary<Transform, float> aiDistanceToNextWaypoint = new Dictionary<Transform, float>();

    private void Start()
    {
        if (IsServer)
        {
            // Initialize dictionaries for each connected player
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                if (playerObject != null)
                {
                    playerTransforms[clientId] = playerObject.transform;
                    playerPositions[clientId] = 0;
                    playerWaypointProgress[clientId] = 0;
                    playerDistanceToNextWaypoint[clientId] = 0f;
                }
            }

            // Initialize AI players
            foreach (var ai in aiPlayers)
            {
                aiPositions[ai] = 0;
                aiWaypointProgress[ai] = 0;
                aiDistanceToNextWaypoint[ai] = 0f;
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (playerObject != null)
            {
                playerTransforms[clientId] = playerObject.transform;
                playerPositions[clientId] = 0;
                playerWaypointProgress[clientId] = 0;
                playerDistanceToNextWaypoint[clientId] = 0f;
            }
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (IsServer)
        {
            playerTransforms.Remove(clientId);
            playerPositions.Remove(clientId);
            playerWaypointProgress.Remove(clientId);
            playerDistanceToNextWaypoint.Remove(clientId);
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdatePlayerProgress();
            UpdateAIPlayerProgress();
            UpdateAllPositions();
        }
    }

    private void UpdatePlayerProgress()
    {
        foreach (var kvp in playerTransforms)
        {
            ulong clientId = kvp.Key;
            Transform player = kvp.Value;

            // Get current waypoint index for this player
            int currentWaypoint = playerWaypointProgress[clientId];
            
            // Calculate distance to next waypoint
            Vector3 nextWaypointPos = waypoints[currentWaypoint].position;
            float distanceToWaypoint = Vector3.Distance(player.position, nextWaypointPos);
            playerDistanceToNextWaypoint[clientId] = distanceToWaypoint;

            // Check if player has reached the waypoint
            if (distanceToWaypoint < 5f) // 5 units threshold for waypoint completion
            {
                playerWaypointProgress[clientId] = (currentWaypoint + 1) % waypoints.Length;
            }
        }
    }

    private void UpdateAIPlayerProgress()
    {
        foreach (var ai in aiPlayers)
        {
            // Get current waypoint index for this AI
            int currentWaypoint = aiWaypointProgress[ai];
            
            // Calculate distance to next waypoint
            Vector3 nextWaypointPos = waypoints[currentWaypoint].position;
            float distanceToWaypoint = Vector3.Distance(ai.position, nextWaypointPos);
            aiDistanceToNextWaypoint[ai] = distanceToWaypoint;

            // Check if AI has reached the waypoint
            if (distanceToWaypoint < 5f) // 5 units threshold for waypoint completion
            {
                aiWaypointProgress[ai] = (currentWaypoint + 1) % waypoints.Length;
            }
        }
    }

    private void UpdateAllPositions()
    {
        // Create a list of all participants (players and AI)
        var allParticipants = new List<(Transform transform, float progress, float distance)>();
        
        // Add human players
        foreach (var kvp in playerTransforms)
        {
            ulong clientId = kvp.Key;
            Transform player = kvp.Value;
            int waypointProgress = playerWaypointProgress[clientId];
            float distanceToNext = playerDistanceToNextWaypoint[clientId];
            allParticipants.Add((player, waypointProgress, distanceToNext));
        }

        // Add AI players
        foreach (var ai in aiPlayers)
        {
            int waypointProgress = aiWaypointProgress[ai];
            float distanceToNext = aiDistanceToNextWaypoint[ai];
            allParticipants.Add((ai, waypointProgress, distanceToNext));
        }

        // Sort all participants by progress
        var sortedParticipants = allParticipants
            .OrderByDescending(p => p.progress * 1000 - p.distance)
            .ToList();

        // Update positions for all participants
        for (int i = 0; i < sortedParticipants.Count; i++)
        {
            var participant = sortedParticipants[i];
            if (playerTransforms.ContainsValue(participant.transform))
            {
                // It's a human player
                var clientId = playerTransforms.First(x => x.Value == participant.transform).Key;
                playerPositions[clientId] = i + 1;
            }
            else
            {
                // It's an AI player
                aiPositions[participant.transform] = i + 1;
            }
        }
    }

    public int GetPlayerPosition(ulong clientId)
    {
        return playerPositions.ContainsKey(clientId) ? playerPositions[clientId] : -1;
    }

    public int GetAIPosition(Transform aiTransform)
    {
        return aiPositions.ContainsKey(aiTransform) ? aiPositions[aiTransform] : -1;
    }

    public string GetPositionText(int position)
    {
        switch (position)
        {
            case 1: return "1st";
            case 2: return "2nd";
            case 3: return "3rd";
            default: return position + "th";
        }
    }
} 