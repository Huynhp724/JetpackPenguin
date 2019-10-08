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
    

    private PlayerController pc;
    private PlayerController.State state; //idle = 0, dashing = 1, flapping = 2, clinging = 3
    private AudioSource aud;
    private bool lastgrounded;
    private Vector3 curpos;
    private Vector3 lastpos;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        aud = GetComponent<AudioSource>();
        lastgrounded = true;
        curpos = transform.position;
        lastpos = transform.position;
    }


    private void FixedUpdate()
    {


        if (pc.onGround && pc.GetCurrentState() != PlayerController.State.Dashing) {
            if (pc.getHoriInput() != 0 || pc.getVertInput() != 0)
            {
                if (aud.clip != walkCycle)
                {
                    aud.clip = walkCycle;
                    aud.volume = 1.0f;
                    aud.Play();
                }
            }
            if (aud.clip == walkCycle && pc.getHoriInput() == 0 && pc.getVertInput() == 0) {
                aud.Stop();
            }
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

        if (ReInput.players.GetPlayer(0).GetButtonDown("Pause"))
        {
            aud.volume = 1.0f;
            aud.PlayOneShot(pause);
        }

        state = pc.GetCurrentState();
        if (state == PlayerController.State.Dashing)
        {


            
        }
        else if (state == PlayerController.State.Flapping)
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
