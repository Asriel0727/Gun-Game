using System.Collections.Generic;
using UnityEngine;

public class AutoWayPoint : MonoBehaviour
{
    public List<AutoWayPoint> connected = new List<AutoWayPoint>();

    static AutoWayPoint[] waypoints = new AutoWayPoint[0];

    void Awake()
    {
        RebuildWaypointList();
    } 

    static public AutoWayPoint FindClosest(Vector3 pos)
    {
        AutoWayPoint closest = null;
        float closestDistance = 100000.0f;

        foreach (var cur in waypoints)
        {
            var distance = Vector3.Distance(cur.transform.position, pos);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = cur;
            }
        }

        return closest;
    }

    [ContextMenu("Update Waypoints")]
    void UpdateWaypoints()
    {
        RebuildWaypointList();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Waypoint.tif");
    }

    void OnDrawGizmosSelected()
    {
        if (waypoints.Length == 0)
            RebuildWaypointList();

        foreach (var p in connected)
        {
            if (p == null)
            {
                connected.Remove(p);
                break;
            }

            if (Physics.Linecast(transform.position, p.transform.position))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, p.transform.position);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, p.transform.position);
            }
        }
    }

    void RebuildWaypointList()
    {
        waypoints = FindObjectsOfType<AutoWayPoint>();
        foreach (var point in waypoints)
            point.RecalculateConnectedWaypoints();
    }

    void RecalculateConnectedWaypoints()
    {
        connected.Clear();

        foreach (var other in waypoints)
        {
            if (other == this)
                continue;

            if (!Physics.Linecast(transform.position, other.transform.position))
                connected.Add(other);
        }
    }
}
