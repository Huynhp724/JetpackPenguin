﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

// This script handles any specials non-movemnet abilities such as throwing an ice bomb.
public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] GameObject endPointPrefab;
    [SerializeField] float maxRange = 20f;
    [SerializeField] Vector3 raycastOffput = new Vector3(0, 3, 0);
    [SerializeField] Transform throwStartPoint;
    [SerializeField] GameObject snowBomb;
    [SerializeField] float bombVelocity = 1f;
    [SerializeField] float timeBetweenThrows = .5f;
    [SerializeField] GameObject playerModel;
    [SerializeField] float throwVelocity = 5f;
    [SerializeField] float throwAngle = 45f;

    private GameObject throwEndPoint = null;
    private float timeOfLastThrow = 0f;
    private Player player;
    private LaunchArcRenderer lineRenderer;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        lineRenderer = throwStartPoint.GetComponentInChildren<LaunchArcRenderer>();
    }

    void Update()
    {
        if(player.GetButton("Aim Bomb"))
        {
            AimSnowBomb();
        }
        else if(player.GetButtonUp("Aim Bomb"))
        {
            lineRenderer.UnrenderArc();
        }
    }

    
    // This function allows the player to aim a snowbomb throw. While holding the aim button the player can ajust their aim the same as moving the camera. There is a max range to the throw.
    private void AimSnowBomb()
    {
        lineRenderer.RenderArc(throwVelocity, throwAngle);

        if (player.GetButtonDown("Throw Bomb") && timeOfLastThrow + timeBetweenThrows <= Time.time)
        {
            ThrowSnowBomb();
        }
    }
    
    /// <summary>
    /// Instansiates and throws the snow bomb at the point targeted at.
    /// </summary>
    private void ThrowSnowBomb()
    {
        timeOfLastThrow = Time.time;
        GameObject currentBomb = Instantiate(snowBomb, throwStartPoint.position, throwStartPoint.rotation);
        float radianAngle = Mathf.Deg2Rad * throwAngle;
        
        float xVelocity = Mathf.Cos(radianAngle) * throwVelocity;
        float yVelocity = Mathf.Sin(radianAngle) * throwVelocity;
        currentBomb.GetComponent<Rigidbody>().velocity = throwStartPoint.transform.TransformDirection(xVelocity, yVelocity, 0);
    }

    /*
    //TODO: Have Pluck look in the direction of the aim. Consider calling a function in PlayerMovement to properly rotate the player.
    // This function allows the player to aim a snowbomb throw. While holding the aim button the player can ajust their aim the same as moving the camera. There is a max range to the throw.
    private void AimSnowBomb()
    {
        RaycastHit hit;
        Vector3 raycastStart = playerModel.transform.position + raycastOffput;
        Vector3 endPointPosition;
        if (Physics.Raycast(raycastStart, playerModel.transform.TransformDirection(Vector3.forward), out hit))
        {
            endPointPosition = hit.point;
        }
        else
        {
            endPointPosition = Vector3.zero;
        }

        // If the raycast hit is out of the max throw range then calculate a point in the same direction but at max range.
        if(Vector3.Distance(endPointPosition, throwStartPoint.position) >= maxRange)
        {
            //float rangeVariance = (Input.GetAxis(gameManager.charge) + 1.0f) / 2.0f;
            endPointPosition = throwStartPoint.position + maxRange * Vector3.Normalize(endPointPosition - throwStartPoint.position); // V0 + du, where u is the normalize vector of V1 - V0
            if (Physics.Raycast(endPointPosition, Vector3.down, out hit))
            {
                endPointPosition -= (Vector3.up * hit.distance);
            }
        }

        if (throwEndPoint == null)
        {
            throwEndPoint = Instantiate(endPointPrefab, endPointPosition, transform.rotation);
        }

        throwEndPoint.transform.position = endPointPosition;

        //transform.LookAt(throwEndPoint.transform.position);

        if (player.GetButtonDown("Throw Bomb") && timeOfLastThrow + timeBetweenThrows <= Time.time)
        {
            ThrowSnowBomb();
        }
    }
    */
}