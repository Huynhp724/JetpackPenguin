using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{

    public Transform oldParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerControl = other.GetComponentInParent<PlayerController>();
        if (playerControl)
        {
            print("Player on rotating platform");
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
            print("Player off rotating platform");
            playerControl.transform.parent.parent = null;
        }
    }
}
