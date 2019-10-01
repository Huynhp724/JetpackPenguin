using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        curpos = this.gameObject.transform.position;
        state = pc.GetCurrentState();
        if (state == PlayerController.State.Idle)
        { //if state is idle
            if (!pc.onGround && lastgrounded)
            {
                //aud.clip = jump;
                lastgrounded = false;
                aud.PlayOneShot(jump);
                aud.volume = 0.4f;
            }
            
            if (pc.onGround) {
                if (curpos == lastpos)
                {
                    aud.Stop();
                    //Debug.Log("Not Moving");


                }
                else
                {
                    //Debug.Log("Moving");
                    if (!aud.isPlaying)
                    {
                        aud.clip = walkCycle;
                        aud.Play();
                        aud.volume = 1.3f;
                    }
                }
                lastgrounded = true;
                
            }
        }
        else if (state == PlayerController.State.Dashing)
        {


            if (!aud.isPlaying || aud.clip != slide)
            {
                aud.clip = slide;
                aud.Play();
                aud.volume = 1.0f;
            }
        }
        else if (state == PlayerController.State.Flapping)
        {
            if (!aud.isPlaying || aud.clip != hoverFlap)
            {
                aud.clip = hoverFlap;
                aud.Play();
                aud.volume = 0.4f;
            }
        }

        lastpos = this.gameObject.transform.position;
    }
}
