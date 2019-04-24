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
    [SerializeField] float jumpDuration = 1.0f;
    [SerializeField] AnimationCurve jumpCurve = new AnimationCurve();

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
        if (myNavMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(jumpDuration));
        }

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

    IEnumerator Jump(float duration)
    {
        OffMeshLinkData data = myNavMeshAgent.currentOffMeshLinkData;
        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + (myNavMeshAgent.baseOffset * Vector3.up); // takes into account the base offset

        float time = 0.0f;

        while(time < duration)
        {
            float t = time / duration;
            myNavMeshAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + jumpCurve.Evaluate(t) * Vector3.up;
            time += Time.deltaTime;
            yield return null;
        }

        myNavMeshAgent.CompleteOffMeshLink(); // give control of navigation back to unity
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
