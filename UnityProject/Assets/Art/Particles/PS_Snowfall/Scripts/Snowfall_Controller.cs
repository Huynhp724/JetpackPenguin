using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowfall_Controller : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("Controls the emission of the entire Particle System")]
    public bool particleSwitch = true;// this controls the emitters in the particle systems, on false, they will stop emitting.
    [Tooltip("Setting true will reduce the amount of snow being emitted by half. Only works when particle switch is true")]
    public bool lightSnow = false;// halves the emission rate of the particle emitters
    [Tooltip("The rate of emission from each particle system (going too high could be a bad time, 20 is default)")]
    [SerializeField] private float emissionRate = 20;
    [Space(20)]

    [Header("References")]
    [SerializeField] private ParticleSystem particleGroup1;
    [SerializeField] private ParticleSystem particleGroup2;
    [SerializeField] private ParticleSystem particleGroup3;


    private void Update()
    {
        var mainMod1 = particleGroup1.emission;
        var mainMod2 = particleGroup2.emission;
        var mainMod3 = particleGroup3.emission;

        if (particleSwitch)
        {
            if (!lightSnow)
            {
                mainMod1.rateOverTime = emissionRate;
                mainMod2.rateOverTime = emissionRate;
                mainMod3.rateOverTime = emissionRate;
            }
            if(lightSnow)
            {
                mainMod1.rateOverTime = emissionRate/2;
                mainMod2.rateOverTime = emissionRate/2;
                mainMod3.rateOverTime = emissionRate/2;
            }
        }
        else
        {
            mainMod1.rateOverTime = 0;
            mainMod2.rateOverTime = 0;
            mainMod3.rateOverTime = 0;
        }
    }

}
