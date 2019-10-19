using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PauseSound : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip pause;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ReInput.players.GetPlayer(0).GetButtonDown("Pause"))
        {
            aud.volume = 1.0f;
            aud.PlayOneShot(pause);
        }
    }
}
