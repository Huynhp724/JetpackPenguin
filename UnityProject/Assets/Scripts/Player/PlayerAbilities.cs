using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

// This script handles any specials non-movement abilities such as throwing an ice bomb.
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
    [SerializeField] float projectileSpeedMultiplier = 2f; //This is actually a gravity multiplier for an arc throw, but for the sake of the game designers they can adjust this for speed. 
    [Tooltip("Max distance a target can be from Pluck and still be targetable.")]
    [SerializeField] float maxTargetRange = 40f;
    [Tooltip("The UI element that indicates which target is selected.")]
    [SerializeField] RawImage targetPointUI;
    [SerializeField] Transform grabCastT;
    [Tooltip("How far from Pluck the player can pick an ice block.")]
    [SerializeField] float pickUpRange = 3f;
    [Tooltip("How far and fast a picked up iceblock will be thrown.")]
    [SerializeField] float blockThrowVelocity = 10f;
    [SerializeField] float blockOverHeadMinimum = 1f;


    private float throwVelocity;
    private float throwAngle = 45f;
    private float timeOfLastThrow = 0f;
    private Player player;
    private LaunchArcRenderer lineRenderer;
    private PlayerController playerController;
    private float xVelocity;
    private float yVelocity;
    private List<GameObject> targets = new List<GameObject>();
    private GameObject target;
    private Quaternion throwStartingPointOrgRotation;
    private Camera mainCamera;
    private bool isAiming = false;
    private bool hasFirstTarget = false;
    private bool stickHasMoved = false;
    private Transform heldIceBlock;
    private float sphereCastRadius = 0.2f;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        lineRenderer = throwStartPoint.GetComponent<LaunchArcRenderer>();
        playerController = GetComponent<PlayerController>();
        throwStartingPointOrgRotation = throwStartPoint.localRotation;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Press once to aim, press again to exit aiming.
        if(heldIceBlock == null && player.GetButtonDown("Aim Bomb"))
        {
            isAiming = !isAiming;
        }

        // While aiming
        if (isAiming && playerController.GetCurrentState() != PlayerController.State.Dashing)
        {
            if (targets.Count > 0)
            {
                AimSnowBombWithTarget();
            }
            else
            {
                AimSnowBombWithArc();
            }
        }
        // Not aiming
        else
        {
            lineRenderer.UnrenderArc();
            throwAngle = maxThrowAngle;
            playerController.setIsAiming(false);
            targetPointUI.enabled = false;
            hasFirstTarget = false;

            //Spherecast for an ice block and pick up if button is pressed.
            RaycastHit iceBlockHit;
            if (heldIceBlock == null && Physics.SphereCast(transform.position, sphereCastRadius, playerModel.transform.forward, out iceBlockHit, pickUpRange, LayerMask.GetMask("IceBlock")) && player.GetButtonDown("Throw Bomb"))
            {
                pickupIceBlock(iceBlockHit.transform);
            }
            //If already holding a block then throw the block if the button is pressed.
            else if(heldIceBlock != null && player.GetButtonDown("Throw Bomb"))
            {
                throwIceBlock();
            }
            else if(heldIceBlock != null && player.GetButtonDown("Drop"))
            {
                dropIceBlock();
            }
        }
    }

    //Put the iceblock above the player's head.
    private void pickupIceBlock(Transform iceblock)
    {
        float iceblockDisplacement = (1 + (iceblock.GetComponent<IceBlock>().getHalfSize() / 2));
        if(iceblockDisplacement <= blockOverHeadMinimum)
        {
            iceblockDisplacement = blockOverHeadMinimum;
        }
        iceblock.position = transform.position + (Vector3.up * iceblockDisplacement);
        iceblock.rotation = playerModel.transform.rotation;
        iceblock.SetParent(playerModel.transform);
        iceblock.GetComponent<Rigidbody>().useGravity = false;
        iceblock.GetComponent<Rigidbody>().isKinematic = true;
        iceblock.GetComponent<BoxCollider>().enabled = false;
        iceblock.GetComponent<IceBlock>().pickedUp = true;
        heldIceBlock = iceblock;
        playerController.setHoldingBlock(true);
    }

    //Throw the iceblock
    private void throwIceBlock()
    {
        Rigidbody iceRB = heldIceBlock.GetComponent<Rigidbody>();
        iceRB.useGravity = true;
        iceRB.isKinematic = false;
        heldIceBlock.GetComponent<BoxCollider>().enabled = true;
        heldIceBlock.SetParent(null);
        heldIceBlock.GetComponent<IceBlock>().pickedUp = false;
        iceRB.velocity = playerModel.transform.forward * blockThrowVelocity + GetComponent<Rigidbody>().velocity;
        heldIceBlock = null;
        playerController.setHoldingBlock(false);
    }

    //Drop the iceblock
    private void dropIceBlock()
    {
        Rigidbody iceRB = heldIceBlock.GetComponent<Rigidbody>();
        iceRB.useGravity = true;
        iceRB.isKinematic = false;
        heldIceBlock.GetComponent<BoxCollider>().enabled = true;
        heldIceBlock.SetParent(null);
        heldIceBlock.GetComponent<IceBlock>().pickedUp = false;
        heldIceBlock.transform.position += playerModel.transform.forward * 1.5f;
        heldIceBlock = null;
        playerController.setHoldingBlock(false);
    }

    // This function allows the player to aim a snowbomb throw at an object with the targetable script attached to it. Pressing the throw bomb button will throw the bomb directly at the target.
    private void AimSnowBombWithTarget()
    {
        if (!hasFirstTarget)
        {
            target = getClosestTargetToCenter();
            hasFirstTarget = true;
        }
        
        // If there are more than 1 target on screen allow the play to move between them.
        if(targets.Count > 1)
        {
            if(stickHasMoved && player.GetAxisTimeActive("Camera X") <= 0)
            {
                stickHasMoved = false;
            }
            // Move current target right
            if(!stickHasMoved && player.GetAxis("Camera X") > .25f)
            {
                stickHasMoved = true;
                target = getClosestTargetRight(target);
            }
            // Move current target left
            else if (!stickHasMoved && player.GetAxis("Camera X") < -0.25f)
            {
                stickHasMoved = true;
                target = getClosestTargetLeft(target);
            }
        }

        lineRenderer.UnrenderArc();

        //Aim at the target.
        throwVelocity = 10f * projectileSpeedMultiplier;
        Vector3 currentTargetPosition = target.transform.position;
        Vector3 targetDir = currentTargetPosition - transform.position;
        playerController.setIsAiming(true);
        playerController.rotateTo(new Vector3(targetDir.x, 0, targetDir.z));
        throwStartPoint.LookAt(currentTargetPosition);

        // Set the UI target element to the screen position of the target.
        targetPointUI.enabled = true;
        targetPointUI.transform.position = mainCamera.WorldToScreenPoint(currentTargetPosition);

        // Throw the bomb.
        if (player.GetButtonDown("Throw Bomb") && timeOfLastThrow + timeBetweenThrows <= Time.time)
        {
            ThrowSnowBombWithTarget();
        }
    }

    // Function used to find the target on screen closest to the center. Used when first entering aim mode.
    private GameObject getClosestTargetToCenter()
    {
        GameObject closestTarget = targets[0];
        float closestTargetDistance = Mathf.Abs((Screen.width / 2) - mainCamera.WorldToScreenPoint(closestTarget.transform.position).x);

        foreach (GameObject target in targets)
        {
            float currentTargetDistance = Mathf.Abs((Screen.width / 2) - mainCamera.WorldToScreenPoint(target.transform.position).x);
            if (currentTargetDistance < closestTargetDistance)
            {
                closestTargetDistance = currentTargetDistance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    // Find the closest target to the right of the current target.
    private GameObject getClosestTargetRight(GameObject currentTarget)
    {
        GameObject closestTarget = currentTarget;
        float currentTargetX = mainCamera.WorldToScreenPoint(currentTarget.transform.position).x;
        float closestTargetX = 99999f;
        foreach(GameObject target in targets)
        {
            float screenPosX = mainCamera.WorldToScreenPoint(target.transform.position).x;
            if (!target.Equals(currentTarget) && screenPosX > currentTargetX && screenPosX < closestTargetX)
            {
                closestTargetX = screenPosX;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    // Find the closest target to the left of the current target.
    private GameObject getClosestTargetLeft(GameObject currentTarget)
    {
        GameObject closestTarget = currentTarget;
        float currentTargetX = mainCamera.WorldToScreenPoint(currentTarget.transform.position).x;
        float closestTargetX = 0;
        foreach (GameObject target in targets)
        {
            float screenPosX = mainCamera.WorldToScreenPoint(target.transform.position).x;
            if (!target.Equals(currentTarget) && screenPosX < currentTargetX && screenPosX > closestTargetX)
            {
                closestTargetX = screenPosX;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    // Throw the bomb at the target. Currently have the projectile move without arc (no gravity).
    private void ThrowSnowBombWithTarget()
    {
        timeOfLastThrow = Time.time;
        GameObject currentBomb = Instantiate(snowBomb, throwStartPoint.position, throwStartPoint.rotation);
        currentBomb.GetComponent<SnowBomb>().setGravityMultiplier(0);
        currentBomb.GetComponent<Rigidbody>().velocity = currentBomb.transform.forward * throwVelocity;
    }

    // This function allows the player to aim a snowbomb throw. By holding the adjust throw button the arc will increase till max angle. Pressing the throw bomb button will throw the bomb.
    private void AimSnowBombWithArc()
    {
        throwStartPoint.localRotation = throwStartingPointOrgRotation;
        playerController.setIsAiming(false);
        targetPointUI.enabled = false;
        hasFirstTarget = false;

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
            ThrowSnowBombWithArc();
        }
    }

    // While called this method decreases the angle of the throw until reachs the min angle. This will increase the distance travelled.
    private void AdjustAim()
    {
        throwAngle = Mathf.MoveTowards(throwAngle, minThrowAngle, adjustThrowRate);
    }

    // Instantiates a snow bomb with the same velcoity of the projected arc.
    private void ThrowSnowBombWithArc()
    {
        timeOfLastThrow = Time.time;
        GameObject currentBomb = Instantiate(snowBomb, throwStartPoint.position, throwStartPoint.rotation);
        float radianAngle = Mathf.Deg2Rad * throwAngle;
        float xVelocity = Mathf.Cos(radianAngle) * throwVelocity;
        float yVelocity = Mathf.Sin(radianAngle) * throwVelocity;
        currentBomb.GetComponent<SnowBomb>().setGravityMultiplier(projectileSpeedMultiplier);
        currentBomb.GetComponent<Rigidbody>().velocity = throwStartPoint.transform.TransformDirection(xVelocity, yVelocity, 0);
    }

    // Public function that the script targetable can use to add itself to targets on screen.
    public void addToTargets(GameObject target)
    {
        print(target + " has been added to targets");
        targets.Add(target);
    }

    // Public function that the script targetable can use to remove itself to targets on screen.
    public void removeFromTargets(GameObject target)
    {
        print(target + " has been removed to targets");
        targets.Remove(target);
    }

    public float getMaxTargetRange()
    {
        return maxTargetRange;
    }
}
