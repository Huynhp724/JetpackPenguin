﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script handles the individual behavior of an ice bomb object.
public class SnowBomb : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}