using UnityEngine;
using UnityEngine.AI;

public class aiController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing!");
            return;
        }

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned!");
            return;
        }

        // Set initial destination
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (agent == null || waypoints.Length == 0) return;

        // Check if we've reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            currentWaypointIndex++;
        }
        else
        {
            // Reset to first waypoint when we reach the end
            currentWaypointIndex = 0;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
