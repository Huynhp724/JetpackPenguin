using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    [SerializeField] GameObject objectToTrigger;
    [SerializeField] bool disableObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<PlayerController>())
        {
            if(disableObject)
            {
                objectToTrigger.SetActive(false);
            }
            else
            {
                objectToTrigger.SetActive(true);
            }
        }
    }
}
