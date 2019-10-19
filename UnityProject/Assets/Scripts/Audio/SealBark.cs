using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealBark : MonoBehaviour
{

    [SerializeField] AudioClip sealBark;

    private AudioSource asource;
    private float delay;
    private float cycle;


    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
        cycle = Random.Range(20.0f, 150.0f);
        delay = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delay += 1.0f;
        if (delay > cycle) {
            if (asource.pitch > 0.5f && asource.pitch < 1.5f)
            {
                asource.pitch += Random.Range(-.1f, .1f);
            }
            else if (asource.pitch <= 0.5f)
            {
                asource.pitch += .2f;
            }
            else {
                asource.pitch -= .2f;
            }
            asource.PlayOneShot(sealBark);
            delay = 0;
            cycle = cycle = Random.Range(20.0f, 150.0f);
        }
    }
}
