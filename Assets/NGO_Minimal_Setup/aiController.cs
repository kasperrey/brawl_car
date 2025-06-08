using UnityEngine;
using UnityEngine.AI;

public class aiController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isMovingToWaypoint = false;
    private float waypointReachedThreshold = 2f;

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

        // Only check for waypoint completion if we're currently moving to one
        if (isMovingToWaypoint && !agent.pathPending)
        {
            // Check if we've reached the current waypoint
            if (agent.remainingDistance <= waypointReachedThreshold)
            {
                isMovingToWaypoint = false;
                MoveToNextWaypoint();
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            currentWaypointIndex++;
            isMovingToWaypoint = true;
        }
        else
        {
            // Reset to first waypoint when we reach the end
            currentWaypointIndex = 0;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            isMovingToWaypoint = true;
        }
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
        isMovingToWaypoint = false;
        
        if (agent != null && waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }
}
