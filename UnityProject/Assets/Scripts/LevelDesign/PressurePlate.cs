using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] int[] layersThatCanPress;

    private IceBlock iceBlock;
    private Renderer rend;
    private int objectsOnPlate = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Have to check if the Ice Block was picked up by the player since OnTriggerExit won't read this.
        if(iceBlock && iceBlock.pickedUp)
        {
            if (--objectsOnPlate <= 0)
                unPressed();
            iceBlock = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(int layer in layersThatCanPress)
        {
            if (other.gameObject.layer == layer)
            {
                pressed(other.transform);
                objectsOnPlate++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (int layer in layersThatCanPress)
        {
            if (other.gameObject.layer == layer)
            {
                if(--objectsOnPlate <= 0)
                    unPressed();

                if (other.gameObject.layer == 13)
                    iceBlock = null;
            }
        }
    }

    private void pressed(Transform collision)
    {
        if(collision.gameObject.layer == 13)
            iceBlock = collision.transform.GetComponent<IceBlock>();
        door.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        rend.enabled = false;
    }

    private void unPressed()
    {
        door.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        rend.enabled = true;
    }
}
