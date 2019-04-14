using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;  // needed for the Editor class
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor // for Editor scripts
{
    // --------------------------------------------------------------------------------
    // Name	:	OnInspectorGUI (Override)
    // Desc	:	Called by Unity Editor when the Inspector needs repainting for an
    //			AIWaypointNetwork Component
    // --------------------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        // draws what would have been automatically drawn by unity if this method wasn't defined
        DrawDefaultInspector();

        // gets access to the selected AIWaypointNetwork
        AIWaypointNetwork network = (AIWaypointNetwork)target; // c# uses c-style casting

        // display the enumeration
        var displayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode", network.GetDisplayMode());
        network.SetDisplayMode(displayMode);

        if (network.GetDisplayMode() == PathDisplayMode.Paths) // only display these options when in paths mode
        {
            //int numWaypoints = network.GetWaypoints().Count;
            int numWaypoints = network.GetWaypoints().Count;
            var startIdx = EditorGUILayout.IntSlider("Waypoint Start", network.GetUiStartIndex(), 0, numWaypoints - 1);
            network.SetUiStartIndex(startIdx);
            var endIdx = EditorGUILayout.IntSlider("Waypoint End", network.GetUiEndIndex(), 0, numWaypoints - 1);
            network.SetUiEndIndex(endIdx);
        }
    }

    // --------------------------------------------------------------------------------
    // Name	:	OnSceneGUI
    // Desc	:	Implementing this functions means the Unity Editor will call it when
    //			the Scene View is being repainted. This gives us a hook to do our
    //			own rendering to the scene view.
    // --------------------------------------------------------------------------------
    void OnSceneGUI()
    {
        // gets access to the selected AIWaypointNetwork
        AIWaypointNetwork network = (AIWaypointNetwork)target; // c# uses c-style casting

        // loop through each waypoint and display it's name and build up an array
        // for drawing the path 
        List<Vector3> waypointPositions = new List<Vector3>();

        int counter = 0;
        var waypoints = network.GetWaypoints();
        foreach (var waypoint in waypoints)
        {
            if (waypoint == null)
            {
                continue;
            }

            waypointPositions.Add(waypoint.position);

            Handles.Label(waypoint.position, "Waypoint " + counter.ToString());
            ++counter;
        }

        var displayMode = network.GetDisplayMode();
        if (displayMode == PathDisplayMode.Connections)
        {

            // add the first waypoint again for a complete loop
            waypointPositions.Add(waypointPositions[0]); // add the first waypoint again for a complete loop

            // draw the lines
            Handles.color = Color.green;
            Handles.DrawPolyLine(waypointPositions.ToArray());
        }
        else if (displayMode == PathDisplayMode.Paths)
        {
            // instantiate a new NavMeshPath to search
            var navMeshPath = new NavMeshPath();

            // get the two waypoints to calculate a path for
            int startIdx = network.GetUiStartIndex();
            int endIdx = network.GetUiEndIndex();

            var fromWaypoint = waypoints[startIdx];
            var toWaypoint = waypoints[endIdx];

            if (fromWaypoint != null && toWaypoint != null)
            {
                Vector3 fromPosition = fromWaypoint.position;
                Vector3 toPosition = toWaypoint.position;

                // query the navmesh to calculate the path
                if (NavMesh.CalculatePath(fromPosition, toPosition, NavMesh.AllAreas, navMeshPath))
                {
                    // draw the array of points in the calculated path
                    Handles.color = Color.yellow;
                    Handles.DrawPolyLine(navMeshPath.corners);
                }
            }
        }
    }
}
