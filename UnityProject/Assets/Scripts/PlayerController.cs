using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerController : MonoBehaviour
{
    enum State { Idle, Flapping, Dashing }

    public float moveSpeed = 15.0f;
    public float dashSpeed = 30.0f;
    public float rotateSpeed = 2f;
    public float rocketSpeed = 10.0f;
    public float chargeSpeed = 1f;
    public float steeringForce = 5f;        //how strong you can turn while flying
    public float jumpForce = 50.0f;
    public float gravityScale = 1f;

    State myState = State.Idle;

    public float maxFuel = 100f;

    public float currentCharge = 0f;
    public float currentFuel = 100f;
    public float currentDashSpd = 30f;

    bool hasDashed = false;

    public CharacterController controller;
    private Vector3 moveDirection;
    public Transform pivot;
    public GameObject playerModel;

    public Text chargeText;

    ParticleSystem[] flames;
    public GameObject jetPack;

    private Player player;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
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
        }

        //If currently dashing (whether in the air or on the ground)
        if (myState == State.Dashing)
        {
            if (currentDashSpd < 0)
            {
                currentDashSpd = 0;
            }
            moveDirection = transform.up * currentDashSpd;

            //Jump to get out of dashing
            if (player.GetButtonDown("Jump"))
            {
                myState = State.Idle;
                transform.RotateAround(transform.position, playerModel.transform.right, -90);
                currentDashSpd = dashSpeed;
                tempY += jumpForce * 1.25f;
                hasDashed = true;
            }
        }

        //Carries over the y velocity
        moveDirection.y = tempY;

        //On the Ground
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
                /*
                 * REPLACE HARD CODED FRICTION LATER
                 */

                currentDashSpd -= .2f;
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
            }
        }
        //In the Air
        else
        {
        
            if (myState == State.Idle)
            {
                //Press A to enter flapping state while idle in the air if you are falling
                if (player.GetButtonDown("Jump") && moveDirection.y < 0)
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
                if (player.GetButtonDown("Jump"))
                {
                    moveDirection.y -= Physics.gravity.y * gravityScale * .85f * Time.deltaTime;
                }
                //Let go of A to return to free fall
                if (player.GetButtonDown("Jump"))
                {
                    myState = State.Idle;
                }
            }
            else if(myState == State.Dashing)
            {
                /*
                 * REPLACE HARD CODED FRICTION LATER
                 */

                currentDashSpd -= .3f;
            }
        }

        //Penguin Dash
        if (player.GetButtonDown("Slide") && myState != State.Dashing && !hasDashed)
        {
            myState = State.Dashing;
            transform.RotateAround(transform.position, playerModel.transform.right, 90);
            moveDirection = transform.up * dashSpeed;
            /*
             * REPLACE HARD CODED MINI INITIAL DASH JUMP LATER
             */
            moveDirection.y += 5f;
        }

        //Hovering with the jetpack
        if (player.GetButton("Hover") && currentFuel > 0)
        {
            if(!flames[0].isPlaying) EmitFlames(true);
            if (myState != State.Dashing)
            {
                myState = State.Idle;
                moveDirection.y += rocketSpeed;
            }
            //Hovering while dashing
            else
            {
                moveDirection += transform.up * rocketSpeed;
                currentDashSpd += rocketSpeed;
                transform.RotateAround(transform.position, playerModel.transform.forward, -1 * player.GetAxis("Move Horizontal") * steeringForce * Time.deltaTime);
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
                    moveDirection += transform.up * currentCharge;
                    moveDirection.y -= Physics.gravity.y * gravityScale * Time.deltaTime;
                    currentDashSpd += currentCharge;
                }
                currentCharge = 0;
            }
        }

        if (currentFuel < 0) currentFuel = 0;
        chargeText.text = "Fuel: " + currentFuel.ToString("F2");

        //Add Gravity
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
   
        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
      
        //Rotate model as you turn
        if((player.GetAxis("Move Horizontal") != 0 || player.GetAxis("Move Vertical") != 0) && myState != State.Dashing)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
