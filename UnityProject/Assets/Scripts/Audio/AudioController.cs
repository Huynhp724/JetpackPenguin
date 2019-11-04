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

    public AudioScript audioScript;
    public AudioScript audioScript2;


    private PlayerController pc;
    private PlayerController.State state; //idle = 0, dashing = 1, flapping = 2, clinging = 3
    private AudioSource aud;
    private bool lastgrounded;
    private Vector3 curpos;
    private Vector3 lastpos;

    private bool isWalking;
    private bool isDashing;

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
    }


    private void FixedUpdate()
    {


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
            aud.volume = 0.2f;
            aud.PlayOneShot(jump);
        }
        if (pc.getPressJumpInAir()) {
            aud.volume = 0.2f;
            aud.PlayOneShot(jump);
        }
        if (pc.getDashJump())
        {
            aud.volume = 0.2f;
            aud.PlayOneShot(jump);
        }
        if (pc.getHovering() && aud.clip != jetHover)
        {
            aud.clip = jetHover;
            aud.volume = 0.6f;
            aud.Play();
        }
        else if(aud.clip == jetHover && !pc.getHovering() ){
            
            //aud.Stop(); //This is commented out because pc.getHovering() is not constant. The player controller does not keep track properly. Known Bug.
        }
        if (pc.getChargeRelease()) {
            aud.volume = 0.8f;
            aud.PlayOneShot(jetJump);
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

        if (aud.clip == hoverFlap && pc.onGround) {
            aud.Stop();
        }

        
    }
}
