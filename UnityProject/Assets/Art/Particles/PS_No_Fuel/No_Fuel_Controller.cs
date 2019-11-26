using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class No_Fuel_Controller : MonoBehaviour
{
    [Header("StartJetPackRechargeParticles();")]
    [Header("StopJetPackRechargeParticles();")]
    [Header("Public Functions:")]
    [Space]
    [SerializeField] private ParticleSystem rechargePS;
    [SerializeField] private bool isDebugControls = false;
    private bool isPlaying = false;

    void Update()
    {
        Debugger();
    }

    void Debugger()
    {
        if (isDebugControls)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
            {
                StartJetPackRechargeParticles();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StopJetPackRechargeParticles();
            }
        }
    }

    public void StartJetPackRechargeParticles()
    {
        rechargePS.Play();
        isPlaying = true;
    }

    public void StopJetPackRechargeParticles()
    {
        rechargePS.Stop();
        isPlaying = false;
    }
}
