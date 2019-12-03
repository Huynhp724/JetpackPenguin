using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Rewired;

public class AudioController : MonoBehaviour
{
    public AudioClip pause;

    [Header("Pluck Audio Clips")] //Collection of AudioClips to play for Pluck's sounds.
    public AudioClip walkCycle;
    public AudioClip runCycle;
    public AudioClip jump;
    public AudioClip doubleJump;
    public AudioClip hoverFlap;
    public AudioClip slide;


    [Header("Jetpack Audio Clips")] //Collection of AudioClips to play for Jetpack sounds.
    public AudioClip jetJump;
    public AudioClip jetHover;
    public AudioClip jetCharge;

    public AudioScript audioScript;
    public AudioScript audioScript2;
    public AudioScript audioScript3;


    private PlayerController pc;
    private PlayerController.State state; //idle = 0, dashing = 1, flapping = 2, clinging = 3
    private AudioSource aud;
    private bool lastgrounded;
    private Vector3 curpos;
    private Vector3 lastpos;

    private bool isWalking;
    private bool isDashing;
    private WorldManager wm;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
        pc = GetComponent<PlayerController>();
        aud = GetComponent<AudioSource>();
        lastgrounded = true;
        curpos = transform.position;
        lastpos = transform.position;
        isWalking = false;
        isDashing = false;
        player = ReInput.players.GetPlayer(0);
        wm = FindObjectOfType<WorldManager>();
    }


    private void FixedUpdate()
    {


        if (player.GetButton("Hover") && pc.currentFuel > 0)
        {

            if (aud.clip != jetHover)
            {
                aud.clip = jetHover;
                aud.volume = 0.6f;
                aud.Play();
            }
        }
        else
        {
            if (player.GetButton("Charge") && pc.currentFuel > 0 && wm.getFinalCrystals() > 1)
            {

                if (aud.clip != jetCharge)
                {
                    aud.clip = jetCharge;
                    aud.volume = 0.8f;
                    aud.Play();
                }
            }
            else
            {

                aud.Stop();
                aud.clip = null;
            }
        }

        if (pc.onGround && pc.GetCurrentState() != PlayerController.State.Dashing)
        {

            if (pc.getHoriInput() != 0 || pc.getVertInput() != 0)
            {
                if (!isWalking)
                {
                    isWalking = true;
                    audioScript.PlaySound(0);
                }
                if (aud.clip != walkCycle)
                {
                    aud.clip = walkCycle;



                }
            }
            else
            {
                isWalking = false;
                audioScript.StopSound();
            }
            if (aud.clip == walkCycle && pc.getHoriInput() == 0 && pc.getVertInput() == 0)
            {
                aud.Stop();
            }
        }
        else {
            isWalking = false;
            audioScript.StopSound();
        }

        if (pc.getPressJump()) {
            //jumpScript.PlaySound(0);
        }
        if (pc.getPressJumpInAir()) {
            //audioScript3.PlaySound(0);
        }
        if (pc.getDashJump())
        {
            //audioScript3.PlaySound(0);
        }
        if (pc.getHovering() && aud.clip != jetHover)
        {
            //aud.clip = jetHover;
            //aud.volume = 0.6f;
            //aud.Play();
        }
        else if(aud.clip == jetHover && !pc.getHovering() ){
            
            //aud.Stop(); //This is commented out because pc.getHovering() is not constant. The player controller does not keep track properly. Known Bug.
        }

    }

    // Update is called once per frame
    void Update()
    {
        

        
        state = pc.GetCurrentState();
        if (state == PlayerController.State.Dashing)
        {
            if (pc.onGround)
            {
                if (!isDashing)
                {

                    audioScript2.PlaySound(0);
                    isDashing = true;
                }
                
            }

        }
        else {
            audioScript2.FadeSoundOut();
            isDashing = false;
            
        }

        if (state == PlayerController.State.Flapping)
        {
            if (!aud.isPlaying || aud.clip != hoverFlap)
            {
                aud.clip = hoverFlap;
                aud.volume = 0.4f;
                aud.Play();
                
            }
        }

        

    }
}
