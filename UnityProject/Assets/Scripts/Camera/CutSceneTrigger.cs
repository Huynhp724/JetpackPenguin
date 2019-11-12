using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    private bool hasTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasTrigger)
        {
            hasTrigger = true;
            GetComponentInParent<CutScene>().playCutScene();
        }
    }
}
