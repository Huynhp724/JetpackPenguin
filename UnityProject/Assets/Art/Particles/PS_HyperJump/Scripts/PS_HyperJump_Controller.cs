using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_HyperJump_Controller : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.1f;
    [Tooltip("if set true, the parent object of the particle effect will be destroyed after it plays")]
    public bool isDestoryAfterPlay = true; //if set true, the parent object of the particle effect will be destroyed after it plays
    private float timer;
    [Space(20)]
    [Header("Initialization")]
    [SerializeField] private ParticleSystem particle1;
    [SerializeField] private ParticleSystem particle2;
    [SerializeField] private ParticleSystem particle3;
    [Space(20)]
    [Header("Debug")]
    [SerializeField] bool isSpaceBarDebugEnabled = false;

    private void Awake()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        if (isDestoryAfterPlay)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                Destroy(gameObject);
            }
        }
        //Debug stuff
        if (isSpaceBarDebugEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playHyperJumpFX();
            }
        }
    }

    public void playHyperJumpFX()
    {
        particle1.Play();
        particle2.Play();
        particle3.Play();
    }
}
