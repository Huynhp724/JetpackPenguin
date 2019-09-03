using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

// This script handles any specials non-movemnet abilities such as throwing an ice bomb.
public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] Transform throwStartPoint;
    [SerializeField] GameObject snowBomb;
    [SerializeField] GameObject playerModel;
    [SerializeField] float timeBetweenThrows = .5f;
    [SerializeField] float minThrowVelocity = 5f;
    [SerializeField] float MaxThrowVelocity = 10f;
    [SerializeField] float throwAngle = 45f;
    [SerializeField] float adjustThrowRate = .1f;

    private float throwVelocity;
    private float timeOfLastThrow = 0f;
    private Player player;
    private LaunchArcRenderer lineRenderer;
    private PlayerController playerController;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        lineRenderer = throwStartPoint.GetComponent<LaunchArcRenderer>();
        playerController = GetComponent<PlayerController>();
        throwVelocity = minThrowVelocity;
    }

    void Update()
    {
        if(player.GetButton("Aim Bomb") && playerController.GetCurrentState() != PlayerController.State.Dashing)
        {
            AimSnowBomb();
        }
        else //if(player.GetButtonUp("Aim Bomb"))
        {
            lineRenderer.UnrenderArc();
            throwVelocity = minThrowVelocity;
        }
    }

    
    // This function allows the player to aim a snowbomb throw. By holding the adjust throw button the arc will increase till max velocity. Pressing the throw bomb button will throw the bomb.
    private void AimSnowBomb()
    {
        if (player.GetButton("Adjust Throw"))
        {
            AdjustAim();
        }

        lineRenderer.RenderArc(throwVelocity, throwAngle);

        if (player.GetButtonDown("Throw Bomb") && timeOfLastThrow + timeBetweenThrows <= Time.time)
        {
            ThrowSnowBomb();
        }
    }

    // While called this method increases the velocity of the throw until reached the max velocity.
    private void AdjustAim()
    {
        throwVelocity = Mathf.MoveTowards(throwVelocity, MaxThrowVelocity, adjustThrowRate);
    }
    
    // Instantiates a snow bomb with the same velcoity of the projected arc.
    private void ThrowSnowBomb()
    {
        timeOfLastThrow = Time.time;
        GameObject currentBomb = Instantiate(snowBomb, throwStartPoint.position, throwStartPoint.rotation);
        float radianAngle = Mathf.Deg2Rad * throwAngle;
        float xVelocity = Mathf.Cos(radianAngle) * throwVelocity;
        float yVelocity = Mathf.Sin(radianAngle) * throwVelocity;
        currentBomb.GetComponent<Rigidbody>().velocity = throwStartPoint.transform.TransformDirection(xVelocity, yVelocity, 0);
    }
}
