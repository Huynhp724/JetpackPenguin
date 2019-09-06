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
    [Tooltip("Time player has to wait between a throw. In real seconds.")]
    [SerializeField] float timeBetweenThrows = .5f;
    [Tooltip("Angle in degrees. decreasing will increase the MAX distance thrown.")]
    [Range(.1f, 90)]
    [SerializeField] float minThrowAngle = 35f;
    [Tooltip("Angle in degrees. Increasing will increase the MIN distance thrown.")]
    [Range(10, 90)]
    [SerializeField] float maxThrowAngle = 60f;
    [Tooltip("How high the peak of a throw will be.")]
    [SerializeField] float arcHeight = 10f;
    [Tooltip("How fast the player can adjust the throw by holding the adjust button.")]
    [SerializeField] float adjustThrowRate = .1f;
    [Tooltip("How fast the projectile will go through the arc.")]
    [SerializeField] float projectileSpeedMultiplier = 2f; //This is actually a gravity multiplier, but for the sake of the game designers they can adjust this for speed. 

    private float throwVelocity;
    private float throwAngle = 45f;
    private float timeOfLastThrow = 0f;
    private Player player;
    private LaunchArcRenderer lineRenderer;
    private PlayerController playerController;
    private float xVelocity;
    private float yVelocity;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        lineRenderer = throwStartPoint.GetComponent<LaunchArcRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if(player.GetButton("Aim Bomb") && playerController.GetCurrentState() != PlayerController.State.Dashing)
        {
            AimSnowBomb();
        }
        else
        {
            lineRenderer.UnrenderArc();
            throwAngle = maxThrowAngle;
        }
    }

    
    // This function allows the player to aim a snowbomb throw. By holding the adjust throw button the arc will increase till max angle. Pressing the throw bomb button will throw the bomb.
    private void AimSnowBomb()
    {       
        if (player.GetButton("Adjust Throw"))
        {
            AdjustAim();
        }

        // Calculate the velocity based off the desired max height and the speed (gravity) of the projectile.
        float radianAngle = Mathf.Deg2Rad * throwAngle;
        throwVelocity = Mathf.Sqrt((2 * arcHeight * Math.Abs(Physics.gravity.y * projectileSpeedMultiplier)) / (Mathf.Sin(radianAngle) * Mathf.Sin(radianAngle)));

        lineRenderer.RenderArc(throwVelocity, throwAngle, Mathf.Abs(Physics.gravity.y * projectileSpeedMultiplier));

        if (player.GetButtonDown("Throw Bomb") && timeOfLastThrow + timeBetweenThrows <= Time.time)
        {
            ThrowSnowBomb();
        }
    }

    // While called this method decreases the angle of the throw until reachs the min angle. This will increase the distance travelled.
    private void AdjustAim()
    {
        throwAngle = Mathf.MoveTowards(throwAngle, minThrowAngle, adjustThrowRate);
    }
    
    // Instantiates a snow bomb with the same velcoity of the projected arc.
    private void ThrowSnowBomb()
    {
        timeOfLastThrow = Time.time;
        GameObject currentBomb = Instantiate(snowBomb, throwStartPoint.position, throwStartPoint.rotation);
        float radianAngle = Mathf.Deg2Rad * throwAngle;
        float xVelocity = Mathf.Cos(radianAngle) * throwVelocity;
        float yVelocity = Mathf.Sin(radianAngle) * throwVelocity;
        currentBomb.GetComponent<SnowBomb>().setGravityMultiplier(projectileSpeedMultiplier);
        currentBomb.GetComponent<Rigidbody>().velocity = throwStartPoint.transform.TransformDirection(xVelocity, yVelocity, 0);
    }
}
