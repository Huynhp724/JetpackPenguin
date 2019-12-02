using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] int[] layersThatCanPress;
    public bool open = false;
    public bool close = false;

    private IceBlock iceBlock;
    private Renderer rend;
    [SerializeField] int objectsOnPlate = 0;
    Animator gateAnim;

    void Start()
    {
        rend = GetComponent<Renderer>();
        gateAnim = door.GetComponent<Animator>();

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

        if (open && !gateAnim.GetBool("Open")) {
            gateAnim.SetBool("Open", true);
            //open = false;
        }
        else if(!open && gateAnim.GetBool("Open"))
        {
            gateAnim.SetBool("Open", false);
           //close = false;
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
        //print("Pressed by: " + collision.gameObject);
        AudioScript auds = GetComponent<AudioScript>();
        auds.PlaySound(0);
        if (collision.gameObject.layer == 13)
            iceBlock = collision.transform.GetComponent<IceBlock>();
        //door.SetActive(false);
        //gateAnim.SetTrigger("Open");
        open = true;
        transform.GetChild(0).gameObject.SetActive(true);
        rend.enabled = false;
    }

    private void unPressed()
    {
        //door.SetActive(true);
        gateAnim.SetTrigger("Close");
        transform.GetChild(0).gameObject.SetActive(false);
        rend.enabled = true;
        open = false;
    }
}
