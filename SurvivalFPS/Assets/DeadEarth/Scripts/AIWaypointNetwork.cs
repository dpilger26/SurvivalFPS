using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIWaypointNetwork : MonoBehaviour 
{
    // configuration parameters
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }
}
