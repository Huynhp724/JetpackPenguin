using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class visualizes in the editor the waypoints and path of a moving platform.
public class DrawPathGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 lastWayPoint = Vector3.zero;
        Vector3 FirstWayPoint = Vector3.zero;
        foreach (Transform waypoint in transform)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);

            if (lastWayPoint != Vector3.zero)
            {
                Gizmos.DrawLine(lastWayPoint, waypoint.position);
            }
            else
            {
                FirstWayPoint = waypoint.position;
            }
            lastWayPoint = waypoint.position;
        }
        Gizmos.DrawLine(lastWayPoint, FirstWayPoint);
    }
}
