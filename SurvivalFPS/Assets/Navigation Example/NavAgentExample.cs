using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// script requires a NavMeshAgent. will auto add one if this script is added to a GameObject
[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    // configuration parameters
    [SerializeField] AIWaypointNetwork waypointNetwork;
    [SerializeField] int currentWaypointIdx = 0;

    // cached references
    private NavMeshAgent myNavMeshAgent;
    private List<Transform> waypoints;

    private void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();

        if (waypointNetwork == null)
        {
            return;
        }

        waypoints = waypointNetwork.GetWaypoints();

        SetNextDestination(false);
    }

    private void Update()
    {
        if ((!myNavMeshAgent.hasPath && !myNavMeshAgent.pathPending) ||
            myNavMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetNextDestination(true);
        }
        else if (myNavMeshAgent.isPathStale)
        {
            SetNextDestination(false);
        }
    }

    private void SetNextDestination(bool incramentIndex)
    {
        if (waypointNetwork == null)
        {
            return;
        }

        int incramentStep = incramentIndex ? 1 : 0;

        Transform nextWaypointTransform = null;
        int nextWaypointIdx = 0;

        while (nextWaypointTransform == null)
        {
            int nextStep = currentWaypointIdx + incramentStep;
            nextWaypointIdx = (nextStep > waypoints.Count - 1) ? 0 : nextStep;
            nextWaypointTransform = waypoints[nextWaypointIdx];
            ++currentWaypointIdx;
        }

        currentWaypointIdx = nextWaypointIdx;
        myNavMeshAgent.destination = nextWaypointTransform.position;
    }
}
