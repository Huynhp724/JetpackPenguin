﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script computes severals points along a projectile path and then creates a line using them. Based off of the tutorial Board To Bits Games on youtube.
[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour
{
    
    [SerializeField] int resolution = 10;

    private LineRenderer lr;
    //private float gravity;
    private float radianAngle;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        //gravity = Mathf.Abs(Physics.gravity.y);
    }

    private void OnValidate()
    {
        if(lr != null && Application.isPlaying)
        {
            //RenderArc();
        }
    }

    void Start()
    {
        UnrenderArc();
    }

    public void UnrenderArc()
    {
        lr.enabled = false;
    }

    // Populating the LineRender with the appropriate settings.
    public void RenderArc(float velocity, float angle, float gravity)
    {
        lr.enabled = true;
        lr.positionCount = (resolution + 1);
        lr.SetPositions(CalculateArcArray(velocity, angle, gravity));
    }

    // Create an array of vector3 positions
    private Vector3[] CalculateArcArray(float velocity, float angle, float gravity)
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for(int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(velocity, t, maxDistance, gravity);
        }

        return arcArray;
    }

    // Calculate each height and distance of each vertex.
    private Vector3 CalculateArcPoint(float velocity, float t, float maxDistance, float gravity)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
