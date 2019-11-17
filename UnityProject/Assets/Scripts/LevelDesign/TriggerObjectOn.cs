using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjectOn : MonoBehaviour
{
    public GameObject obj;
    public string tag;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag)) {
            activateObject();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tag))
        {
            deactivateObject();
        }
    }

    public void activateObject()
    {
        obj.SetActive(true);
    }

    public void deactivateObject()
    {
        obj.SetActive(false);
    }
}
