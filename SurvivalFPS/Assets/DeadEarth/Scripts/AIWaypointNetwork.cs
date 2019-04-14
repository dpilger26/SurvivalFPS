using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PathDisplayMode { None, Connections, Paths };

public class AIWaypointNetwork : MonoBehaviour 
{
    // configuration parameters
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    // member parameters
    private PathDisplayMode displayMode = PathDisplayMode.Connections;
    private int uiStart = 0;
    private int uiEnd = 0;


    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }

    public PathDisplayMode GetDisplayMode()
    {
        return displayMode;
    }

    public void SetDisplayMode(PathDisplayMode pathDisplayMode)
    {
        displayMode = pathDisplayMode;
    }

    public int GetUiStartIndex()
    {
        return uiStart;
    }

    public void SetUiStartIndex(int startIndex)
    {
        uiStart = startIndex;
    }

    public int GetUiEndIndex()
    {
        return uiEnd;
    }
    public void SetUiEndIndex(int endIndex)
    {
        uiEnd = endIndex;
    }
}
