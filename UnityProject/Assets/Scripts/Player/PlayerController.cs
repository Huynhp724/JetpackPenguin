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
    public float rotateSpeed = 2f;          
    public float rocketSpeed = 10.0f;       //how strong the rocket is
    public float chargeSpeed = 1f;
    public float steeringForce = 5f;        //how strong you can turn while flying
    [Range(0.0f, 100.0f)]
    public float jumpForce = 50.0f;         //how high you jump
    [Range(0.0f, 1.0f)] 
    public float airDashLength = 0.8f;      //how far you dash in the air (it's technically the percentage of slow down before you start falling. For example, a value of .8 means you start falling
                                            //when your air speed slows down to 80% of your initial dash speed. The closer to 1, the shorter your air dash.
    [Range(0.0f, 3.0f)]
    public float dashCancelJump = 1.0f;     //a modifier of how strong your dash cancel jump is (a value of 1.25 means 125% stronger than your normal jump force)
    [Range(0.0f, 1.0f)]
    public float flapStrength = 0.0f;       //the percentage in which flapping reduces the gravity (a value closer to 1 means gravity is more reduced)
    [Range(0.0f, 1.0f)]
    public float dashGravity = 0.0f;        //the percentage in which dashing recuces the effectiveness of gravity (a value of .5 means gravity is 50% effective, .25 is 25%, etc.) 
                                            //this is different from flapStrength because here the closer the value is to 0, the more gravity is reduced
    [Range(0.0f, 10.0f)]
    public float gravityScale = 1f;         //how strong the gravity is for Pluck

    [SerializeField]
    private GameObject slideSphere;
    private GameObject currentSlideSphere;

    State myState = State.Idle;

    public float maxFuel = 100f;

    public float currentCharge = 0f;
    public float currentFuel = 100f;

    public bool onLedge = false;
    private Vector3 ledgePos;

    public Transform grabCastT;

    bool hasDashed = false;

    public CharacterController controller;
    private Vector3 moveDirection;
    public Transform pivot;
    public GameObject playerModel;

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
        Debug.Log(myState);
        float tempY = moveDirection.y;
        
        //Regular Movement
        if (myState != State.Dashing)
        {
            moveDirection = (transform.forward * player.GetAxis("Move Vertical") * moveSpeed) + (transform.right * player.GetAxis("Move Horizontal") * moveSpeed);
            moveDirection = moveDirection.normalized * moveSpeed;
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

        //If currently dashing (whether in the air or on the ground) 
        if (myState == State.Dashing)
        {
           
            //Jump to get out of dashing
            if (player.GetButtonDown("Jump"))
            {
                Destroy(currentSlideSphere);
                myState = State.Idle;
                tempY = jumpForce * dashCancelJump;
                hasDashed = true;
            }
        }

        //Carries over the y velocity
        moveDirection.y = tempY;

        //ON THE GROUND
        if (controller.isGrounded)
        {
            moveDirection.y = 0;
            hasDashed = false;
            if (myState == State.Flapping)
            {
                myState = State.Idle;
            }
            else if(myState == State.Dashing)
            {
                currentSlideSphere.GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityScale * dashGravity, 0));
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
                moveDirection.y += jumpForce;
                anim.SetTrigger("Jump");
            }
        }
        //IN THE AIR
        else
        {
        
            if (myState == State.Idle)
            {
                //Press A to enter flapping state while idle in the air if you are falling
                if (player.GetButton("Jump") && moveDirection.y < 0)
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
                if (player.GetButton("Jump"))
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
                Vector3 tempVel = currentSlideSphere.GetComponent<Rigidbody>().velocity;
                //Debug.Log(tempVel.magnitude);
                if (Mathf.Abs(tempVel.magnitude) > dashSpeed * airDashLength) 
                {
                    currentSlideSphere.GetComponent<Rigidbody>().useGravity = false;
                }
                else
                {
                    currentSlideSphere.GetComponent<Rigidbody>().useGravity = true;
                    currentSlideSphere.GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityScale * dashGravity, 0));
                    //Debug.Log(currentSlideSphere.GetComponent<Rigidbody>().velocity.y);
                }
            }

            //ledge grabbing
            RaycastHit ledgeGrabHit;
            if (!onLedge && Physics.Raycast(grabCastT.position, transform.forward, out ledgeGrabHit, 2f, LayerMask.GetMask("LedgeGrab")))
            {
                onLedge = true;
                GrabableLedge ledge = ledgeGrabHit.transform.GetComponent<GrabableLedge>();
                controller.transform.position = ledgePos = ledge.GetGrabPosition(ledgeGrabHit.point);
                controller.transform.rotation = ledge.GetGrabRotation(ledgeGrabHit.point);
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

        //PENGUIN DASH START
        if (player.GetButtonDown("Slide") && myState != State.Dashing && !hasDashed)
        {
            myState = State.Dashing;
            //transform.RotateAround(transform.position, playerModel.transform.right, 90);

            currentSlideSphere = Instantiate(slideSphere, transform.position, Quaternion.identity);
            currentSlideSphere.GetComponent<Rigidbody>().mass = gameObject.GetComponent<Rigidbody>().mass;
            currentSlideSphere.GetComponent<Rigidbody>().drag = gameObject.GetComponent<Rigidbody>().drag;
            currentSlideSphere.GetComponent<Rigidbody>().angularDrag = gameObject.GetComponent<Rigidbody>().angularDrag;
            currentSlideSphere.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward * dashSpeed, ForceMode.Impulse);

            //moveDirection = playerModel.transform.forward * dashSpeed;
            /*
             * REPLACE HARD CODED MINI INITIAL DASH JUMP LATER
             */
            moveDirection.y += jumpForce * dashCancelJump;
        }

        //Hovering with the jetpack
        if (player.GetButton("Hover") && currentFuel > 0)
        {
            if(!flames[0].isPlaying) EmitFlames(true);
            if (myState != State.Dashing)
            {
                myState = State.Idle;
                if (moveDirection.y < 0) moveDirection.y = 0;
                moveDirection.y += rocketSpeed;
            }
            //Hovering while dashing
            else
            {
                if (currentSlideSphere != null)
                {
                    Rigidbody sphereRB = currentSlideSphere.GetComponent<Rigidbody>();
                    sphereRB.AddForce(playerModel.transform.forward * rocketSpeed, ForceMode.Impulse);
                }
                transform.RotateAround(transform.position, playerModel.transform.up, player.GetAxis("Move Horizontal") * steeringForce * Time.deltaTime);
                moveDirection.y -= Physics.gravity.y * gravityScale * Time.deltaTime;
            }
            currentFuel -= .5f;
        }
        else if (player.GetButton("Hover") == false && flames[0].isPlaying && currentFuel > 0)
        {
            EmitFlames(false);
        }

        //Charge jumping - Charge
        if (player.GetButton("Charge") && currentFuel > 0)
        {
            currentCharge += chargeSpeed;
            currentFuel -= chargeSpeed;
        }
        //Charge jumping - Release
        else if(!player.GetButton("Charge"))
        {
            if (currentCharge > 0)
            {
                if (!flames[0].isPlaying) EmitFlames(true);
                if (myState != State.Dashing)
                {
                    myState = State.Idle;
                    moveDirection.y += currentCharge;
                }
                //Releasing charge while dashing
                else
                {
                    if (currentSlideSphere != null)
                {
                    Rigidbody sphereRB = currentSlideSphere.GetComponent<Rigidbody>();
                    sphereRB.AddForce(playerModel.transform.forward * currentCharge, ForceMode.Impulse);
                }
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
            controller.transform.position = Vector3.Lerp(controller.transform.position, currentSlideSphere.transform.position + new Vector3(0, 0.5f, 0), 1f);
        }
        else if (!onLedge)
        {
            controller.Move(moveDirection * Time.deltaTime);

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
            controller.transform.position = ledgePos;
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

    public void movePlayer(Vector3 direction)
    {
        controller.Move(direction);
    }
}
