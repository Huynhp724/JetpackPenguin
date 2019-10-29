using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script move a platform from one waypoint to the next.
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] DrawPathGizmo patrolPath;
    [SerializeField] float waypointTolerance = 2f;
    [SerializeField] float timeToWaitAtWaypoints = .5f;
    [SerializeField] float speed = .5f;

    private int nextPathPointIndex = 0;
    private Vector3 nextPathPointPos;
    private float timeAtWaypoint = 0;
    public Transform oldParent;

    void Awake()
    {
        nextPathPointPos = patrolPath.transform.GetChild(nextPathPointIndex).position;
    }

    void Update()
    {
        if (timeAtWaypoint + timeToWaitAtWaypoints <= Time.time)
        {
            // Move the position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, nextPathPointPos, step);

            if (Vector3.Distance(nextPathPointPos, transform.position) < waypointTolerance)
            {
                nextPathPointIndex = (nextPathPointIndex + 1) % patrolPath.transform.childCount;
                nextPathPointPos = patrolPath.transform.GetChild(nextPathPointIndex).position;
                timeAtWaypoint = Time.time;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerControl = other.GetComponentInParent<PlayerController>();
        if (playerControl)
        {
            print("Player on moving platform");
            //float step = speed * Time.deltaTime; // calculate distance to move
            //playerControl.movePlayer(Vector3.MoveTowards(other.transform.position, nextPathPointPos, step));
            if (oldParent == null)
            {
                oldParent = playerControl.transform.parent.parent;
            }
            playerControl.transform.parent.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController playerControl = other.GetComponentInParent<PlayerController>();
        if (playerControl)
        {
            print("Player on moving platform");
            //float step = speed * Time.deltaTime; // calculate distance to move
            //playerControl.movePlayer(Vector3.MoveTowards(other.transform.position, nextPathPointPos, step));
            playerControl.transform.parent.parent = null;
        }
    }
}
