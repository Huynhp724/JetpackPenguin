using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{

    [SerializeField] AudioClip menuToggle;
    [SerializeField] AudioClip menuCancel;
    [SerializeField] AudioClip menuAccept;

    private AudioSource asource;
   
    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playToggle() {
        asource.PlayOneShot(menuToggle);
    }

    public void playCancel()
    {
        asource.PlayOneShot(menuCancel);
    }
    public void playAccept()
    {
        asource.PlayOneShot(menuAccept);
    }
}
