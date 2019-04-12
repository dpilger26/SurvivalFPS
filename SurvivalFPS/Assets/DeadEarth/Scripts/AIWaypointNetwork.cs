using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PathDisplayMode { None, Connections, Paths };

public class AIWaypointNetwork : MonoBehaviour 
{
    // configuration parameters
    [SerializeField] PathDisplayMode displayMode = PathDisplayMode.Connections;
    [SerializeField] int uiStart = 0;
    [SerializeField] int uiEnd = 0;
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }

    public PathDisplayMode GetDisplayMode()
    {
        return displayMode;
    }

    public int GetUiStartIndex()
    {
        return uiStart;
    }

    public int GetUiEndIndex()
    {
        return uiEnd;
    }
}
