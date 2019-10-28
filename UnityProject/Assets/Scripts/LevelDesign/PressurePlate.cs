using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] int iceBlockLayer;

    private IceBlock iceBlock;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(iceBlock && iceBlock.pickedUp)
        {
            unPressed();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == iceBlockLayer)
        {
            pressed(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == iceBlockLayer)
        {
            unPressed();
        }
    }

    private void pressed(Transform collision)
    {
        iceBlock = collision.transform.GetComponent<IceBlock>();
        door.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        rend.enabled = false;
    }

    private void unPressed()
    {
        iceBlock = null;
        door.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        rend.enabled = true;
    }
}
