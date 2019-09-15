using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public enum State { Idle, Flapping, Dashing }

    public float moveSpeed = 15.0f;
    [Range(0.0f, 100.0f)]
    public float dashSpeed = 30.0f;
    public float dashDrag = 2f;              //how much drag you have while dashing
    private float normalDrag;               //how much drag you have normally
    public float rotateSpeed = 2f;          
    public float rocketSpeed = 10.0f;       //how strong the rocket is
    public float maxHoverVelocityY = 10.0f;  //how fast the rocket can ultimately go when hovering vertically
    public float maxHoverVelocityX = 15.0f;  //how fast the rocket can ultimately go when hovering horizontally
    public float minLeanVelocity = 5.0f;     //the minimum velocity you need to be able to lean in your slide
    public float chargeSpeed = 1f;
    public float leanForce = 10f;           //how much you can lean when sliding
    public float steeringForce = 5f;        //how strong you can turn while flying
    [Range(0.0f, 100.0f)]
    public float jumpForce = 20.0f;         //how high you jump
    [Range(0.0f, 100.0f)]
    public float doubleJumpForce = 10.0f;   //how high your double jump is
    [Range(0.0f, 1.0f)] 
    public float airDashLength = 0.8f;      //how far you dash in the air (it's technically the percentage of slow down before you start falling. For example, a value of .8 means you start falling
                                            //when your air speed slows down to 80% of your initial dash speed. The closer to 1, the shorter your air dash.
    [Range(0.0f, 1.0f)]
    public float dashCancelJump = 1.0f;     //a modifier of how strong your dash cancel jump is (a value of 1.25 means 125% stronger than your normal jump force)
    [Range(0.0f, 1.0f)]
    public float flapStrength = 0.0f;       //the percentage in which flapping reduces the gravity (a value closer to 1 means gravity is more reduced)
    [Range(0.0f, 1.0f)]
    public float dashGravity = 0.0f;        //the percentage in which dashing recuces the effectiveness of gravity (a value of .5 means gravity is 50% effective, .25 is 25%, etc.) 
                                            //this is different from flapStrength because here the closer the value is to 0, the more gravity is reduced
    [Range(0.0f, 10.0f)]
    public float gravityScale = 1f;         //how strong the gravity is for Pluck

    private float groundAngle;
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

    bool hasDashed = false;
    bool hasDoubleJump = true;

    //public CharacterController controller;
    private Vector3 moveDirection;
    public Transform pivot;
    public GameObject playerModel;
    private Rigidbody rb;
    public Collider charCol;
    public LayerMask ground;

    public Text chargeText;

    ParticleSystem[] flames;
    public GameObject jetPack;

    private Player player;
    private Animator anim;

    private bool isAiming = false;

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
        flames = jetPack.GetComponentsInChildren<ParticleSystem>();
        Debug.Log(flames.Length);

    }

    //turns the jetpack particle effects on/off
    void EmitFlames(bool emit)
    {
        Debug.Log("FIRE: " + emit);
        if (emit)
        {
            for(int i = 0; i < flames.Length; i++)
            {
                flames[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < flames.Length; i++)
            {
                flames[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    void Update()
    {
        //Debug.Log(isGrounded());
        //Debug.Log(myState);
        Debug.DrawRay(charCol.transform.position, -Vector3.up * (gameObject.GetComponentInChildren<Collider>().bounds.extents.y + 0.1f));

        float tempY = moveDirection.y;
        float tempX = moveDirection.x;
        float tempZ = moveDirection.z;

        //Regular Movement
        if (myState != State.Dashing)
        {
            //Debug.Log(new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
            //carry over horizontal momentum on ground
            Debug.Log("MOVE SPEED: " + moveSpeed);
            if (new Vector3(moveDirection.x, 0, moveDirection.z).magnitude <= moveSpeed + 0.1f)
            {
                Debug.Log("DON'T CARRY OVER");
                tempX = 0;
                tempZ = 0;
                moveDirection = (transform.forward * player.GetAxis("Move Vertical") * moveSpeed) + (transform.right * player.GetAxis("Move Horizontal") * moveSpeed);
                moveDirection = moveDirection.normalized * moveSpeed;
            }
            else
            {
                Debug.Log("GREATER! " + new Vector3(moveDirection.x, 0, moveDirection.z).magnitude + " is more than " + moveSpeed);
                moveDirection.x = rb.velocity.x;
                moveDirection.z = rb.velocity.z;
            }
            //Debug.Log("X: " + tempX);

            //Carries over the y velocity
            moveDirection.y = tempY;

            //Debug.Log("Move Vertical: " + player.GetAxis("Move Vertical") * moveSpeed);
            //Debug.Log("Move Horizontal : " + player.GetAxis("Move Horizontal") * moveSpeed);
            if (player.GetAxis("Move Vertical") * moveSpeed == 0 && player.GetAxis("Move Horizontal")*moveSpeed == 0)
            {
                anim.SetFloat("Speed", 0f);
            }
            else {
                anim.SetFloat("Speed", 1f);
               
            }
        }


        //ON THE GROUND
        if (isGrounded())
        {
            onGround = true;
            Debug.Log("IS GROUND");
            if(moveDirection.y < 0) moveDirection.y = 0;
            hasDashed = false;
            hasDoubleJump = true;
            if (myState == State.Flapping)
            {
                myState = State.Idle;       
            }
            else if(myState == State.Dashing)
            {
                
            }

            /*
             * CHANGE HARD CODED RECHARGE RATE LATER
             */
            if (!player.GetButton("Charge")){
                currentFuel += 5f;
                if (currentFuel > maxFuel) currentFuel = maxFuel;
            }

            //Jumping
            if (player.GetButtonDown("Jump") && myState == State.Idle) 
            {
                moveOffGround();
                moveDirection.y += jumpForce;
                anim.SetTrigger("Jump");
            }
        }
        //IN THE AIR
        else
        {
            onGround = false;
            if (myState == State.Idle)
            {
                //Press A to double jump and enter flapping state while idle in the air if you are falling
                if ((player.GetButtonDown("Jump") || player.GetButton("Jump") && moveDirection.y < 0) && !player.GetButton("Hover"))
                {
                    if (hasDoubleJump)
                    {
                        moveDirection.y = doubleJumpForce;
                        //reset horizontal momentum with double jump
                        moveDirection.x = 0;
                        moveDirection.z = 0;
                        hasDoubleJump = false;
                    }
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
                    moveDirection.y -= Physics.gravity.y * gravityScale * flapStrength * Time.deltaTime;
                }
                //Let go of A to return to free fall
                if (player.GetButtonUp("Jump"))
                {
                    myState = State.Idle;
                }
            }
            else if(myState == State.Dashing)
            {
                Debug.Log("NO Y");
                //Vector3 tempVel = currentSlideSphere.GetComponent<Rigidbody>().velocity;
                //Debug.Log(tempVel.magnitude);
                if (Mathf.Abs(rb.velocity.magnitude) < dashSpeed * airDashLength) 
                {
                    //currentSlideSphere.GetComponent<Rigidbody>().useGravity = true;
                    //currentSlideSphere.GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityScale * dashGravity, 0));
                    rb.useGravity = true;
                    rb.AddForce(new Vector3(0, Physics.gravity.y * gravityScale, 0));
                    //Debug.Log(currentSlideSphere.GetComponent<Rigidbody>().velocity.y);
                }
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
            //lean in your slide
            if (rb.velocity.magnitude >= minLeanVelocity)
            {
                rb.AddForce(playerModel.transform.right * leanForce * player.GetAxis("Move Horizontal"));

            }
            else
            {
                //Allows player to control the direction they're facing
                transform.RotateAround(transform.position, playerModel.transform.up, player.GetAxis("Move Horizontal") * steeringForce * Time.deltaTime);
            }

            //Jump to get out of dashing
            if (player.GetButtonDown("Jump"))
            {

                Quaternion colRotation = charCol.gameObject.transform.rotation;
                charCol.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            
                //Destroy(currentSlideSphere);
                myState = State.Idle;
                //IF THEY JUMP OUT OF DASHING IN A GROUNDED STATE
                if (isGrounded())
                {
                    moveOffGround();
                    moveDirection.x = rb.velocity.x;
                    moveDirection.z = rb.velocity.z;
                    Debug.Log("MOVE DIRECTION: " + moveDirection);
                    if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > moveSpeed + 0.1f)
                    {
                        //dash jump with a proportionate height to your horizontal momentum
                        moveDirection.y = jumpForce * new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / moveSpeed * dashCancelJump;
                        Debug.Log("RATIO: " + (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / moveSpeed));
                        Debug.Log("JUMP VEL: " + jumpForce * (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / moveSpeed) * dashCancelJump);
                    }
                    else
                    {
                        moveDirection.y = jumpForce * dashCancelJump;
                    }
                }
                else
                {
                    hasDashed = true;
                    moveDirection.y = jumpForce * dashCancelJump;
                }
                rb.drag = normalDrag;
              
            }
        }

        //PENGUIN DASH START
        if (player.GetButtonDown("Slide") && myState != State.Dashing && !hasDashed)
        {
            myState = State.Dashing;
            //rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            Quaternion colRotation = charCol.gameObject.transform.rotation;
            //charCol.gameObject.transform.rotation = Quaternion.Slerp(charCol.gameObject.transform.rotation, Quaternion.Euler(colRotation.eulerAngles.x + 90, colRotation.eulerAngles.y, colRotation.eulerAngles.z), rotateSpeed);
            charCol.gameObject.transform.RotateAround(charCol.gameObject.transform.position, playerModel.transform.right, 90);

            /*currentSlideSphere = Instantiate(slideSphere, transform.position, Quaternion.identity);
            currentSlideSphere.GetComponent<Rigidbody>().mass = gameObject.GetComponent<Rigidbody>().mass;
            currentSlideSphere.GetComponent<Rigidbody>().drag = gameObject.GetComponent<Rigidbody>().drag;
            currentSlideSphere.GetComponent<Rigidbody>().angularDrag = gameObject.GetComponent<Rigidbody>().angularDrag;
            currentSlideSphere.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward * dashSpeed, ForceMode.Impulse);*/
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(playerModel.transform.forward * dashSpeed, ForceMode.Impulse);
            if(!isGrounded()) rb.useGravity = false;

            //moveDirection = playerModel.transform.forward * dashSpeed;

            //moveOffGround();
            //moveDirection.y = jumpForce * dashCancelJump;

            //if(!isGrounded()) rb.drag = dashDrag;
            rb.drag = dashDrag;
            
        }

        //Hovering with the jetpack
        if (player.GetButton("Hover") && currentFuel > 0)
        {
       
            if(!flames[0].isPlaying) EmitFlames(true);
            if (myState != State.Dashing)
            {
                myState = State.Idle;
                if(moveDirection.y < maxHoverVelocityY) moveDirection.y += rocketSpeed;
                //Debug.Log("Y: " + moveDirection.y);
            }
            //Hovering while dashing
            else
            {
                rb.useGravity = false;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.drag = normalDrag;
                /*if (currentSlideSphere != null)
                {
                    //currentSlideSphere.GetComponent<Rigidbody>().useGravity = false;
                    Rigidbody sphereRB = currentSlideSphere.GetComponent<Rigidbody>();
                    sphereRB.AddForce(playerModel.transform.forward * rocketSpeed, ForceMode.Impulse);
                }*/
                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude <= maxHoverVelocityX)
                {
                    //Debug.Log(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
                    rb.AddForce(playerModel.transform.forward * rocketSpeed, ForceMode.Impulse);
                }
                else
                {
                    rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0, rb.velocity.z), maxHoverVelocityX) + new Vector3(0, rb.velocity.y, 0);
                }

                transform.RotateAround(transform.position, playerModel.transform.up, player.GetAxis("Move Horizontal") * steeringForce * Time.deltaTime);
                moveDirection.y -= Physics.gravity.y * gravityScale * Time.deltaTime;
            }
            currentFuel -= .5f;

        }
        else if (player.GetButtonUp("Hover") || currentFuel <= 0)
        {
            //currentSlideSphere.GetComponent<Rigidbody>().useGravity = true;
            //currentSlideSphere.GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityScale * dashGravity, 0));
            if (flames[0].isPlaying && currentFuel > 0)
            {
                EmitFlames(false);
            }
            rb.useGravity = true;
            rb.AddForce(new Vector3(0, Physics.gravity.y * gravityScale, 0));
        }

        //Charge jumping - Charge
        if (player.GetButton("Charge") && currentFuel > 0)
        {
            currentCharge += chargeSpeed;
            currentFuel -= chargeSpeed;
        }
        //Charge jumping - Release
        else if(player.GetButtonUp("Charge"))
        {
            rb.drag = normalDrag;
            if (currentCharge > 0)
            {
                //if (!flames[0].isPlaying) EmitFlames(true);
                if (myState != State.Dashing)
                {
                    myState = State.Idle;
                    moveOffGround();
                    moveDirection.y += currentCharge;
                }
                //Releasing charge while dashing
                else
                {
                    rb.AddForce(playerModel.transform.forward * currentCharge, ForceMode.Impulse);
                    /*if (currentSlideSphere != null)
                    {
                        Rigidbody sphereRB = currentSlideSphere.GetComponent<Rigidbody>();
                        sphereRB.AddForce(playerModel.transform.forward * currentCharge, ForceMode.Impulse);
                    }*/
                }
                currentCharge = 0;
            }
        }

        if (currentFuel < 0) currentFuel = 0;
        chargeText.text = "Fuel: " + currentFuel.ToString("F2");

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
            if ((player.GetAxis("Move Horizontal") != 0 || player.GetAxis("Move Vertical") != 0) && myState != State.Dashing)
            {
                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                if (!isAiming)
                {
                    rotateTo(moveDirection.x, 0, moveDirection.z);
                }
            }
        }
        else
        {
            transform.position = ledgePos;
        }

        if (myState == State.Dashing)
        {
            anim.SetBool("Slide", true);
        }
        else {
            anim.SetBool("Slide", false);
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

    public void rotateTo(float x, float y, float z)
    {
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(x, y, z));
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    private bool isGrounded()
    {
        RaycastHit hit;
        //return Physics.SphereCast(transform.position, gameObject.GetComponentInChildren<Collider>().bounds.extents.x, -Vector3.up, out hit, gameObject.GetComponentInChildren<Collider>().bounds.extents.y + 0.1f);
        if(Physics.CheckSphere(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y/2 + 0.1f, 0), charCol.bounds.extents.y/2, ground))
        {
            return true;
        }

        //Debug.Log("NOT GROUND");
        return false;
    }

    //moves the player up until it is recognized as off the ground
    private void moveOffGround()
    {
        while (isGrounded())
        {
            Debug.Log("MOVING");
            transform.position += Vector3.up * Time.deltaTime;
        }
    }

    void calculateGroundAngle()
    {
        if (!isGrounded())
        {
            groundAngle = 90;
            return;
        }
        //groundAngle = Vector3.Angle(hitinfo.normal, transform.forward);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(charCol.gameObject.transform.position - new Vector3(0, charCol.bounds.extents.y / 2 + 0.1f, 0), charCol.bounds.extents.y/2);
    }

    public void movePlayer(Vector3 direction)
    {
        rb.velocity = direction;
    }

    public float getCurrentSlideSpeed()
    {
        return rb.velocity.magnitude;
    }
}
