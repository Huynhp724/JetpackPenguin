using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformMovePlayer : MonoBehaviour
{
    public Transform oldParent;

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
            playerControl.transform.parent.parent = null;
        }
    }
}
