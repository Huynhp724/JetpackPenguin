using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public enum State { Idle, Flapping, Dashing }

    public float moveForce = 1.0f;             //player move force into max move speed
    public float maxMoveSpeed = 15.0f;         //player's max move speed
    public float maxAirMoveSpeed = 10.0f;          //player's max air move speed
    [Range(0.0f, 100.0f)]
    public float airDashSpeed = 30.0f;         //how fast you dash in the air
    [Range(0.0f, 100.0f)]
    public float dashSpeed = 10.0f;            //how fast you dash on the ground
    public float airDrag = 2f;              //how much drag you have while dashing in the air
    //public float hoverDrag = 2f;            //how much drag when hover flying
    public float chargeDrag = 2f;           //how much drag when charge dashing
    private float normalDrag;               //how much drag you have normally
    public float rotateSpeed = 2f;          
    public float jetpackForce = 10.0f;       //how strong the jetpack is
    public float hoverDashScale = 1.5f;      //modifier of the jetpack hover force when used in dashing
    public float maxHoverVelocityY = 10.0f;  //how fast the jetpack can ultimately go when hovering vertically
    public float maxHoverVelocityX = 15.0f;  //how fast the jetpack can ultimately go when hovering horizontally
    public float minLeanVelocity = 5.0f;     //the minimum velocity you need to be able to lean in your slide
    public float chargeSpeed = 1f;
    public float maxChargeForce = 100.0f;    //the most you can charge before it stops
    public float chargeForceDashScale = 2f;      //how strong the force is (only used during dashing)
    public float leanForce = 10f;           //how much you can lean when sliding
    public float steeringForce = 20f;       //how strong you can turn while flying
    public float turningForce = 5f;        //how fast you can rotate when in dash state
    [Range(0.0f, 100.0f)]
    public float jumpForce = 20.0f;         //how high you jump
    [Range(0.0f, 100.0f)]
    public float doubleJumpForce = 10.0f;   //how high your double jump is
    [Range(0.0f, 1.0f)] 
    public float airDashLength = 0.8f;      //how far you dash in the air (it's technically the percentage of slow down before you start falling. For example, a value of .8 means you start falling
                                            //when your air speed slows down to 80% of your initial dash speed. The closer to 1, the shorter your air dash.
    [Range(0.0f, 1.0f)]
    public float dashJumpMod = 1.0f;     //a modifier of how strong your dash cancel jump is (a value of 1.25 means 125% stronger than your normal jump force)
    [Range(0.0f, 1.0f)]
    public float flapStrength = 0.0f;       //the percentage in which flapping reduces the gravity (a value closer to 1 means gravity is more reduced)
    [Range(0.0f, 1.0f)]
    public float dashGravity = 0.0f;        //the percentage in which dashing recuces the effectiveness of gravity (a value of .5 means gravity is 50% effective, .25 is 25%, etc.) 
                                            //this is different from flapStrength because here the closer the value is to 0, the more gravity is reduced
    [Range(0.0f, 10.0f)]
    public float gravityScale = 1f;         //how strong the gravity is for Pluck
    public float slopePadding = 0.5f;
    public float antiBump = 1f;

    public float groundAngle;
    public float maxGroundAngle;

    [SerializeField]
    //private GameObject slideSphere;
    //private GameObject currentSlideSphere;

    State myState = State.Idle;

    public float maxFuel = 100f;

    public float currentCharge = 0f;
    public float currentFuel = 100f;

    public bool onLedge = false;
    public bool onGround;
    private Vector3 ledgePos;

    public Transform grabCastT;

    bool pressJump = false;
    bool pressJumpInAir = false;
    bool holdFlapping = false;
    bool dashJump = false;
    bool hasDashed = false;
    public bool hasDoubleJump = true;
    bool chargeDashing = false;
    bool initDash = false;
    bool isCharging = false;
    bool isHovering = false;
    bool hoverDashRelease = false;
    bool chargeRelease = false;
    public bool momentumJump = false;

    //public CharacterController controller;
    public Vector3 moveDirection;
    private float vertInput;
    private float horiInput;
    private Vector3 slopeDir;        //direction of slopes
    public Transform pivot;
    public GameObject playerModel;
    private Rigidbody rb;
    public Collider charCol;
    public LayerMask ground;

    public Text chargeText;

    ParticleSystem[] flames;
    public GameObject jetPack;
    public GameObject freezeTrigger;

    private Player player;
    private Animator anim;

    private bool isAiming = false;
    private bool holdingBlock = false;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        //charCol = GetComponent<Collider>();
        normalDrag = rb.drag;

    }

    private void Start()
    {
        flames = gameObject.GetComponentsInChildren<ParticleSystem>();
        Debug.Log(flames.Length);
    }

    //turns the jetpack particle effects on/off
    void EmitFlames(bool emit)
    {
        //Debug.Log("FIRE: " + emit);
        if (emit)
        {
            freezeTrigger.SetActive(true);
            for(int i = 0; i < flames.Length; i++)
            {
                flames[i].Play();
            }
        }
        else
        {
            freezeTrigger.SetActive(false);
            for (int i = 0; i < flames.Length; i++)
            {
                flames[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    //DO ALL THE INPUT READING HERE, MOVE THE RIGIDBODY IN FIXEDUPDATE
    void Update()
    {
        //Debug.Log(isGrounded());
        //Debug.Log(myState);
        Debug.DrawRay(charCol.transform.position, -Vector3.up * (gameObject.GetComponentInChildren<Collider>().bounds.extents.y + 0.1f));

        vertInput = player.GetAxis("Move Vertical");
        horiInput = player.GetAxis("Move Horizontal");

        //ON THE GROUND
        if (isGrounded())
        {
            onGround = true;
            //Debug.Log("IS GROUND");
            hasDashed = false;
            hasDoubleJump = true;
            if (myState == State.Flapping)
            {
                holdFlapping = false;
                myState = State.Idle;
            }
            else if (myState == State.Dashing)
            {

            }

            /*
             * CHANGE HARD CODED RECHARGE RATE LATER
             */
            if (!player.GetButton("Charge")) {
                currentFuel += 5f;
                if (currentFuel > maxFuel) currentFuel = maxFuel;
            }

            //Jumping
            if (player.GetButtonDown("Jump") && myState == State.Idle)
            {
                pressJump = true;
   
            }
        }
        //IN THE AIR
        else
        {
            onGround = false;
            if (myState == State.Idle)
            {
                //Press A to double jump and enter flapping state while idle in the air if you are falling
                if (hasDoubleJump && player.GetButtonDown("Jump") && !player.GetButton("Hover"))
                {
                    pressJumpInAir = true;
                }
                else if (player.GetButton("Jump") && moveDirection.y < 0 && !player.GetButton("Hover") && !pressJumpInAir)
                {
                    myState = State.Flapping;
                }
            }
            else if (myState == State.Flapping)
            {
                /*
                 * Make it so flapping gets weaker after a brief time then you just fall regularly
                 */
                //Hold down A to hover down slowly
                if (player.GetButton("Jump") && moveDirection.y < 0)
                {
                    holdFlapping = true;
                }
                //Let go of A to return to free fall
                if (player.GetButtonUp("Jump"))
                {
                    myState = State.Idle;
                }
            }
            else if(myState == State.Dashing)
            {
                
            }

            //ledge grabbing
            RaycastHit ledgeGrabHit;
            if (!onLedge && Physics.Raycast(grabCastT.position, transform.forward, out ledgeGrabHit, 2f, LayerMask.GetMask("LedgeGrab")))
            {
                onLedge = true;
                GrabableLedge ledge = ledgeGrabHit.transform.GetComponent<GrabableLedge>();
                transform.position = ledgePos = ledge.GetGrabPosition(ledgeGrabHit.point);
                transform.rotation = ledge.GetGrabRotation(ledgeGrabHit.point);
            }

            if (onLedge)
            {
                if (player.GetButtonDown("Jump"))
                {
                    onLedge = false;
                    moveDirection = Vector3.zero;
                    moveDirection += -transform.forward * .25f;
                    moveDirection.y += jumpForce * 1.3f;
                    anim.SetTrigger("Jump");
                }
            }
        }

        //If currently dashing (whether in the air or on the ground) 
        if (myState == State.Dashing)
        {
            anim.SetBool("Slide", true);
            //Jump to get out of dashing
            if (player.GetButtonDown("Jump"))
            {
                dashJump = true;
                anim.SetBool("Slide", false);
              
            }
        }
        else
        {
            anim.SetBool("Slide", false);
        }

        //PENGUIN DASH START
        if (player.GetButtonDown("Slide") && myState != State.Dashing && !hasDashed && !holdingBlock)
        {
            initDash = true;
        }

        //Hovering with the jetpack
        if (player.GetButton("Hover") && currentFuel > 0)
        {
            isHovering = true;
            if(!flames[0].isPlaying) EmitFlames(true);

        }
        else if (player.GetButtonUp("Hover") || currentFuel <= 0)
        {
           
            if (flames[0].isPlaying && currentFuel > 0)
            {
                EmitFlames(false);
            }
            hoverDashRelease = true;
        }

        //Charge jumping - Charge
        if (player.GetButton("Charge") && currentFuel > 0)
        {
            isCharging = true;
            
        }
        //Charge jumping - Release
        else if(player.GetButtonUp("Charge"))
        {
            chargeRelease = true;
        }

        if (currentFuel < 0) currentFuel = 0;
        chargeText.text = "Fuel: " + currentFuel.ToString("F2");

    }

    /// <summary>
    /// MAKE CHANGES TO RIGIDBODY IN FIXEDUPDATE
    /// </summary>
    public void FixedUpdate()
    {
      

        //Regular Movement
        if (myState != State.Dashing)
        {
            //Debug.Log(new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
            //carry over horizontal momentum on ground
            //Debug.Log("MOVE SPEED: " + moveSpeed);
            if (new Vector3(moveDirection.x, 0, moveDirection.z).magnitude <= new Vector3(maxMoveSpeed, 0, maxMoveSpeed).magnitude + 0.1f)
            {
                //Calculate slope angle if on ground
                calculateGroundAngle();
                //rb.drag = normalDrag;
                //Debug.Log("DON'T CARRY OVER");
                if (isGrounded())
                {
                
                    Debug.DrawRay(transform.position, transform.forward + new Vector3(0, -slopeDir.y, 0), Color.green);
                    //Debug.Log("MOVE VERT: " + vertInput);

                    Vector3 targetDir = (transform.forward * vertInput * maxMoveSpeed) + (transform.right * horiInput * maxMoveSpeed) + new Vector3(0, moveDirection.y, 0);
                    if (groundAngle != 0)
                    {
                        //slopeDir = slopeDir.normalized;
                        //targetDir = (slopeDir* vertInput * maxMoveSpeed) + (transform.right * horiInput * maxMoveSpeed) + new Vector3(0, moveDirection.y, 0);
                        //Debug.Log("ADDING: " + slopeDir.y * -1);
                        //targetDir += new Vector3(0, -slopeDir.y, 0);

                        moveDirection.y = rb.velocity.y;
                        //Vector3 temp = slopeDir.normalized * maxMoveSpeed;
                        //Debug.Log(temp.y);
                        //if (slopeDir.y > 0) moveDirection.y = -antiBump;
                  
                        Debug.DrawRay(transform.position, targetDir, Color.red);

                        //targetDir = new Vector3(vertInput, 0, -horiInput);
                        //Vector3 tempDir = Vector3.Cross(slopeDir, targetDir);
                        //targetDir = tempDir * maxMoveSpeed;
                    }
                    moveDirection = Vector3.Lerp(moveDirection, targetDir, moveForce * Time.deltaTime);
                       
                    //moveDirection = moveDirection.normalized * moveSpeed;
                }
                else
                {

                    if (momentumJump && horiInput == 0 && vertInput == 0)
                    {
                        //moveDirection += new Vector3(rb.velocity.x, 0, rb.velocity.z);
                        //Debug.Log("MOMENTUM: " + moveDirection);
                        //moveDirection.x = rb.velocity.x;
                        //moveDirection.z = rb.velocity.z;
                    }
                    else
                    {
                        Vector3 targetDir = (transform.forward * vertInput * maxAirMoveSpeed) + (transform.right * horiInput * maxAirMoveSpeed) + new Vector3(0, moveDirection.y, 0);
                        moveDirection = Vector3.Lerp(moveDirection, targetDir, moveForce * Time.deltaTime);
                        //moveDirection = moveDirection.normalized * airMoveSpeed;
                    }

                }
                //if (moveDirection.magnitude == 0) rb.drag = normalDrag;
            }
            else
            {
                //rb.drag = airDrag;
                //Debug.Log("GREATER! " + new Vector3(moveDirection.x, 0, moveDirection.z).magnitude + " is more than " + moveSpeed);
                moveDirection.x = rb.velocity.x;
                moveDirection.z = rb.velocity.z;
            }
            //Debug.Log("X: " + tempX);


            //Debug.Log("Move Vertical: " + vertInput * moveSpeed);
            //Debug.Log("Move Horizontal : " + horiInput * moveSpeed);
            if (vertInput * moveForce == 0 && horiInput * moveForce == 0)
            {
                anim.SetFloat("Speed", 0f);
            }
            else
            {
                anim.SetFloat("Speed", 1f);

            }
        }

        //Jumping
        if (pressJump)
        {
            moveOffGround();
            moveDirection.y += jumpForce;
            //anim.SetTrigger("Jump");

            pressJump = false;
        }

        //Double jump
        if (pressJumpInAir)
        {

            anim.SetTrigger("Jump");
            moveDirection.y = doubleJumpForce;
            //reset horizontal momentum with double jump
            moveDirection.x = 0;
            moveDirection.z = 0;
            hasDoubleJump = false;
            momentumJump = false;

            pressJumpInAir = false;
        }

        //ON THE GROUND
        if (isGrounded())
        {
            rb.drag = normalDrag;
            if (moveDirection.y < 0)
            {
                moveDirection.y = 0;
                momentumJump = false;
            }

   
        }

        //IN THE AIR
        else
        {
            if (!momentumJump) { rb.drag = airDrag; }
            else { rb.drag = airDrag * .65f; }
            if (myState == State.Idle)
            {
                
            }
            else if (myState == State.Flapping)
            {
                if (holdFlapping)
                {
                    moveDirection.y -= Physics.gravity.y * gravityScale * flapStrength * Time.deltaTime;
                    holdFlapping = false;
                }
            }
            else if (myState == State.Dashing)
            {
                //Debug.Log("NO Y");
                //Vector3 tempVel = currentSlideSphere.GetComponent<Rigidbody>().velocity;
                //Debug.Log(tempVel.magnitude);
                if (rb.velocity.magnitude < airDashSpeed * airDashLength)
                {
                    //currentSlideSphere.GetComponent<Rigidbody>().useGravity = true;
                    //currentSlideSphere.GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityScale * dashGravity, 0));
                    //rb.useGravity = true;
                    rb.AddForce(new Vector3(0, Physics.gravity.y * gravityScale * Time.deltaTime, 0));
                    //rb.drag = normalDrag;
                    //Debug.Log(currentSlideSphere.GetComponent<Rigidbody>().velocity.y);
                }
            }
        }

        //If currently dashing (whether in the air or on the ground) 
        if (myState == State.Dashing)
        {
            Debug.DrawRay(charCol.transform.position, charCol.transform.up * 2f, Color.blue);
            if (chargeDashing) rb.drag = chargeDrag;
            //lean in your slide
            if (rb.velocity.magnitude >= minLeanVelocity)
            {

                rb.AddForce(playerModel.transform.right * leanForce * horiInput * Time.deltaTime);

            }
            else
            {
                //Allows player to control the direction they're facing
                transform.RotateAround(transform.position, playerModel.transform.up, horiInput * turningForce * Time.deltaTime);
                //rb.drag = normalDrag;
            }

            //Jump to get out of dashing
            if (dashJump)
            {

                Quaternion colRotation = charCol.gameObject.transform.rotation;
                charCol.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                chargeDashing = false;

                //Destroy(currentSlideSphere);
                myState = State.Idle;
                //IF THEY JUMP OUT OF DASHING IN A GROUNDED STATE
                if (isGrounded())
                {
                    moveOffGround();
                    moveDirection.x = rb.velocity.x;
                    moveDirection.z = rb.velocity.z;
                    //Debug.Log("MOVE DIRECTION: " + moveDirection);
                    if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > maxMoveSpeed + 0.1f)
                    {
                        //dash jump with a proportionate height to your horizontal momentum
                        moveDirection.y = jumpForce;
                        //Debug.Log("RATIO: " + (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / moveSpeed));
                        //Debug.Log("JUMP VEL: " + jumpForce * (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / moveSpeed) * dashCancelJump);
                        momentumJump = true;
                    }
                    else
                    {
                        moveDirection.y = jumpForce * dashJumpMod;
                    }
                }
                else
                {
                    hasDashed = true;
                    moveDirection.y = jumpForce * dashJumpMod;
                }
                //rb.drag = normalDrag;

                dashJump = false;
            }

        }

        //PENGUIN DASH START
        if (initDash)
        {
            myState = State.Dashing;
            //rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            Quaternion colRotation = charCol.gameObject.transform.rotation;
            //charCol.gameObject.transform.rotation = Quaternion.Slerp(charCol.gameObject.transform.rotation, Quaternion.Euler(colRotation.eulerAngles.x + 90, colRotation.eulerAngles.y, colRotation.eulerAngles.z), rotateSpeed);
            if (slopeDir.y > 0)
            {
                charCol.gameObject.transform.RotateAround(charCol.transform.position, playerModel.transform.right, 90 + groundAngle);
            }
            else
            {
                charCol.gameObject.transform.RotateAround(charCol.transform.position, playerModel.transform.right, 90 - groundAngle);
            }

            /*currentSlideSphere = Instantiate(slideSphere, transform.position, Quaternion.identity);
            currentSlideSphere.GetComponent<Rigidbody>().mass = gameObject.GetComponent<Rigidbody>().mass;
            currentSlideSphere.GetComponent<Rigidbody>().drag = gameObject.GetComponent<Rigidbody>().drag;
            currentSlideSphere.GetComponent<Rigidbody>().angularDrag = gameObject.GetComponent<Rigidbody>().angularDrag;
            currentSlideSphere.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward * dashSpeed, ForceMode.Impulse);*/
            rb.velocity = new Vector3(0, 0, 0);

            //DASH IN THE DIRECTION THE PLAYER'S CONTROL STICK IS
            if (horiInput != 0 || vertInput != 0)
            {
                Vector3 tempDirection = (transform.forward * vertInput) + (transform.right * horiInput);
                tempDirection = tempDirection.normalized;
                if (isGrounded())
                {
                    rb.AddForce(tempDirection * dashSpeed, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(tempDirection * airDashSpeed, ForceMode.Impulse);
                    //rb.useGravity = false;

                }
                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                rotateTo(new Vector3(tempDirection.x, 0, tempDirection.z), 1f);
            }
            else
            {
                if (isGrounded())
                {
                    rb.AddForce(playerModel.transform.forward * dashSpeed, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(playerModel.transform.forward * airDashSpeed, ForceMode.Impulse);
                    //rb.useGravity = false;

                }

            }

            //moveDirection = playerModel.transform.forward * dashSpeed;

            //moveOffGround();
            //moveDirection.y = jumpForce * dashJumpMod;

            //if(!isGrounded()) rb.drag = dashDrag;

            initDash = false;

        }

        //Hovering with the jetpack
        if (isHovering)
        {
            if (myState != State.Dashing)
            {
                moveOffGround();
                myState = State.Idle;
                if (moveDirection.y < maxHoverVelocityY) moveDirection.y += jetpackForce * Time.deltaTime;
                //Debug.Log("Y: " + moveDirection.y);
            }
            //Hovering while dashing
            else
            {
                rb.AddForce(playerModel.transform.right * steeringForce * horiInput * Time.deltaTime);

                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude <= maxHoverVelocityX)
                {
                    //Debug.Log(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
                    rb.AddForce(charCol.transform.up * jetpackForce * hoverDashScale * Time.deltaTime, ForceMode.Impulse);
                }
                else
                {
                    Debug.Log("TOO FAST! CLAMPING HOVER VELOCITY");
                    rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0, rb.velocity.z), maxHoverVelocityX) + new Vector3(0, rb.velocity.y, 0);
                }

                transform.RotateAround(transform.position, playerModel.transform.up, horiInput * turningForce * Time.deltaTime);
                //moveDirection.y -= Physics.gravity.y * gravityScale * Time.deltaTime;
                //rb.useGravity = false;
                //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
            currentFuel -= .5f;

            isHovering = false;
        }
        else if (hoverDashRelease)
        {
            //currentSlideSphere.GetComponent<Rigidbody>().useGravity = true;
            //currentSlideSphere.GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityScale * dashGravity, 0));
           
            //rb.useGravity = true;
            rb.AddForce(new Vector3(0, Physics.gravity.y * gravityScale * Time.deltaTime, 0));
            //rb.drag = normalDrag;

            hoverDashRelease = false;
        }

        //Charge jumping - Charge
        if (isCharging)
        {
            if (currentCharge < maxChargeForce)
            {
                currentCharge += chargeSpeed;
                currentFuel -= chargeSpeed;
            }
            else
            {
                currentCharge = maxChargeForce;
            }
            isCharging = false;
        }

        //Charge jumping - Release
        if (chargeRelease)
        {
            //rb.drag = chargeDrag;
            if (currentCharge > 0)
            {
                //if (!flames[0].isPlaying) EmitFlames(true);
                if (myState != State.Dashing)
                {
                    myState = State.Idle;
                    moveOffGround();
                    moveDirection.y = currentCharge;
                }
                //Releasing charge while dashing
                else
                {
                    chargeDashing = true;
                    rb.AddForce(playerModel.transform.forward * currentCharge * chargeForceDashScale, ForceMode.Impulse);
                    /*if (currentSlideSphere != null)
                    {
                        Rigidbody sphereRB = currentSlideSphere.GetComponent<Rigidbody>();
                        sphereRB.AddForce(playerModel.transform.forward * currentCharge, ForceMode.Impulse);
                    }*/
                }
                currentCharge = 0;
            }
            chargeRelease = false;
        }

        //Add Gravity
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        // Move the controller
        if (myState == State.Dashing)
        {
            //Vector3 dest = (controller.transform.position + currentSlideSphere.transform.position + new Vector3(0, .5f, 0)) / 2;
            //controller.transform.position = Vector3.Lerp(controller.transform.position, currentSlideSphere.transform.position + new Vector3(0, 0.5f, 0), 1f);
        }
        else if (!onLedge)
        {
            //controller.Move(moveDirection * Time.deltaTime);
            rb.velocity = moveDirection;
            //SET LIMITS ON MAX HOVER VELOCITY
            //Debug.Log(rb.velocity.y);
            //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxRocketVelocity);
            //rb.velocity = new Vector3(moveDirection.x, 0, moveDirection.z);
            //rb.AddForce(new Vector3(0, moveDirection.y, 0));
            //rb.MovePosition(transform.position + moveDirection);
            //Debug.Log(moveDirection);

            //Rotate model as you turn
            if ((horiInput != 0 || vertInput != 0) && myState != State.Dashing)
            {
                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                //Debug.Log(pivot.rotation.eulerAngles.y);
                if (!isAiming)
                {
                    rotateTo(new Vector3(moveDirection.x, 0, moveDirection.z));
                }
            }

        }
        else
        {
            transform.position = ledgePos;
        }
    }

    public State GetCurrentState()
    {
        return myState;
    }

    public void setIsAiming(bool aim)
    {
        isAiming = aim;
    }

    public void setHoldingBlock(bool holding)
    {
        holdingBlock = holding;
    }

    public void rotateTo(Vector3 dir)
    {
        Quaternion newRotation = Quaternion.LookRotation(dir);
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    public void rotateTo(Vector3 dir, float speed)
    {
        Quaternion newRotation = Quaternion.LookRotation(dir);
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, speed);
    }

    private bool isGrounded()
    {
        //Debug.DrawRay(charCol.gameObject.transform.position, -Vector3.up * (charCol.bounds.extents.y + 0.1f));
        if (myState != State.Dashing)
        {
            if (Physics.CheckSphere(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y / 2 + 0.1f, 0), charCol.bounds.extents.y / 2, ground))
            {
                return true;
            }
        }
        else
        {
            if (Physics.CheckCapsule (charCol.gameObject.transform.position - -charCol.transform.up * (charCol.bounds.extents.z + charCol.bounds.extents.x) / 4 - new Vector3(0, 0.1f, 0),
                charCol.gameObject.transform.position - charCol.transform.up * (charCol.bounds.extents.z + charCol.bounds.extents.x) / 3 - new Vector3(0, 0.1f, 0),
                charCol.bounds.extents.y, ground))
            {
                return true;
            }
        }
        //Debug.Log("NOT GROUND");
     
        return false;
    }

    //moves the player up until it is recognized as off the ground
    private void moveOffGround()
    {
        while (isGrounded())
        {
            //Debug.Log("MOVING");
            transform.position += Vector3.up * Time.deltaTime;
        }
    }

    private void moveToGround()
    {
        while (!isGrounded())
        {
            //Debug.Log("MOVING");
            transform.position -= Vector3.up * Time.deltaTime;
        }
    }

    void calculateGroundAngle()
    {
        Debug.DrawRay(charCol.transform.position + playerModel.transform.forward, Vector3.down * (charCol.bounds.extents.y / 2 + 0.2f), Color.yellow);
        /*if (!isGrounded())
        {
            groundAngle = 90;
            return;
        }*/

        RaycastHit hit;
        if(Physics.Raycast(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y / 2, 0), Vector3.down, out hit, charCol.bounds.extents.y/2 + slopePadding, ground))
        {
            Debug.Log("HITTING GROUND");
            Debug.DrawRay(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y / 2, 0), Vector3.down * hit.distance, Color.yellow);
            Debug.DrawRay(charCol.gameObject.transform.position + playerModel.transform.forward, Vector3.Cross(hit.normal, playerModel.transform.right) * 1f, Color.blue);
            groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            slopeDir = Vector3.Cross(hit.normal, playerModel.transform.right);
            //Debug.Log("SLOPE: " + slopeDir);
            if(slopeDir.y > 0 && groundAngle != 0)
            {
                Debug.Log("MOVING DOWN");
                moveToGround();
            }
            //Debug.Log("SLOPE: " + slopeDir);
        }
        else
        {
            if (isGrounded())
            {
                Debug.DrawRay(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y / 2, 0), Vector3.down * (charCol.bounds.extents.y / 2 + slopePadding), Color.red);
                Debug.Log("ERROR: ON THE GROUND BUT NOT DETECTING GROUND");
            }
            else
            {
                groundAngle = 0;
            }
        
        }
 
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y / 2 + 0.1f,0), charCol.bounds.extents.y/2);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(charCol.gameObject.transform.position - charCol.transform.up * (charCol.bounds.extents.z + charCol.bounds.extents.x) / 3 - new Vector3(0, 0.1f, 0), charCol.bounds.extents.y);
        //Gizmos.DrawSphere(charCol.gameObject.transform.position - new Vector3(-playerModel.transform.forward.x * 0.5f, charCol.bounds.extents.y + 0.1f, -playerModel.transform.forward.z * 0.05f), (charCol.bounds.extents.z + charCol.bounds.extents.x) / 4);
        Gizmos.DrawSphere(charCol.gameObject.transform.position - -charCol.transform.up * (charCol.bounds.extents.z + charCol.bounds.extents.x) / 4 - new Vector3(0, 0.1f, 0), charCol.bounds.extents.y);

    }

    public void movePlayer(Vector3 direction)
    {
        transform.position = direction;
    }

    public float getCurrentSlideSpeed()
    {
        return rb.velocity.magnitude;
    }
}
