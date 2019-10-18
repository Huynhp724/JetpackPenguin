using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    public GameObject iceWallModel;
    public GameObject waterfallModel;
    public float revertToWaterfallTime;

    public LayerMask waterLayer, iceLayer;

    int ice, waterfall;


    // Start is called before the first frame update
    void Start()
    {
        ice = iceLayer.value;
        waterfall = waterLayer.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeForm(bool water) {
        if (water) // changing to a waterfall
        {
            gameObject.layer = waterfall;
            Debug.Log(LayerMask.LayerToName(gameObject.layer));

            iceWallModel.SetActive(false);
            waterfallModel.SetActive(true);

        }
        else { // changing to a ice wall
            gameObject.layer = ice;
            Debug.Log(LayerMask.LayerToName(gameObject.layer));

            waterfallModel.SetActive(false);
            iceWallModel.SetActive(true);

            StartCoroutine(RevertToWaterfall());
        }
    }

    IEnumerator RevertToWaterfall() {
        yield return new WaitForSeconds(revertToWaterfallTime);

        ChangeForm(true);
    }


}
