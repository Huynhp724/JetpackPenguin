﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script checks if the player slides into the gameobject fast enough, if so then it is detroyed. 
public class BreakableWall : MonoBehaviour
{
    [SerializeField] float playerSpeedThreshhold = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerControl = other.gameObject.GetComponent<PlayerController>();
        if (playerControl)
        {
            /*float playerSpeed = playerControl.getCurrentSlideSpeed();
            if (playerSpeed > playerSpeedThreshhold)
            {
                Destroy(gameObject);
            }*/
        }
    }
}
