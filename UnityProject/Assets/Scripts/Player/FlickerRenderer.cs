using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerRenderer : MonoBehaviour
{
    public GameObject modleRootNode, jumpPackRootNode;
    [Tooltip("Make same as invincibilty frames on Player Health")]
    public float flickerTime;
    [Range(0.01f, 0.75f)]
    public float flickerSpeed;

    bool flickerRenderOn = false, flickerRenderOff = false, timeToFlicker = false;
    float timer, entireFlickerTime;

    SkinnedMeshRenderer[] modelRends;
    MeshRenderer[] packRends;


    // Start is called before the first frame update
    void Start()
    {
        modelRends = modleRootNode.GetComponentsInChildren<SkinnedMeshRenderer>();
        packRends = jumpPackRootNode.GetComponentsInChildren<MeshRenderer>();

        entireFlickerTime = flickerTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToFlicker) {
            entireFlickerTime -= Time.deltaTime;
            FlickerCheck();

            if (entireFlickerTime <= 0f) {
                entireFlickerTime = flickerTime;
                timeToFlicker = false;
                flickerRenderOn = false;
                flickerRenderOff = false;

                TurnRenderers(true);
            }
        }
    }

    void FlickerCheck() {
        if (flickerRenderOff)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = flickerSpeed;
                TurnRenderers(false);
                flickerRenderOff = false;
                flickerRenderOn = true;
            }
        }

        if (flickerRenderOn)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = flickerSpeed;
                TurnRenderers(true);
                flickerRenderOn = false;
                flickerRenderOff = true;
            }
        }
    }

    void TurnRenderers(bool on) {
        for (int i = 0; i < modelRends.Length; i++) {
            modelRends[i].enabled = on;
        }

        for (int i = 0; i < packRends.Length; i++)
        {
            packRends[i].enabled = on;
        }
    }

    public void SetFlickering() {
        timeToFlicker = true;
        flickerRenderOff = true;
    }

    public void SwitchAllRenderers(bool turn) {
        try
        {
            for (int i = 0; i < modelRends.Length; i++)
            {
                modelRends[i].enabled = turn;
            }

            for (int i = 0; i < packRends.Length; i++)
            {
                packRends[i].enabled = turn;
            }
        }
        catch { }
        
    }
}
