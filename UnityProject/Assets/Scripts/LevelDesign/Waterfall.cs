using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    public GameObject iceWallModel;
    public GameObject waterfallModel;
    public float revertToWaterfallTime;


    int ice = 0; // default layer
    int waterfall = 17; // waterfall layer


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeForm(bool water) {
        AudioSource wts = GetComponent<AudioSource>();

        if (water) // changing to a waterfall
        {
            gameObject.layer = waterfall;
            Debug.Log(LayerMask.LayerToName(gameObject.layer));

            iceWallModel.SetActive(false);
            waterfallModel.SetActive(true);
            wts.mute = false;
            

        }
        else { // changing to a ice wall
            gameObject.layer = ice;
            Debug.Log(LayerMask.LayerToName(gameObject.layer));

            waterfallModel.SetActive(false);
            iceWallModel.SetActive(true);
            wts.mute = true;
            StartCoroutine(RevertToWaterfall());
        }
    }

    IEnumerator RevertToWaterfall() {
        yield return new WaitForSeconds(revertToWaterfallTime);

        ChangeForm(true);
    }


}
