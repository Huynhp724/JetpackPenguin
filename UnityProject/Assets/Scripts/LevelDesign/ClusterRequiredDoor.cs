using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClusterRequiredDoor : MonoBehaviour
{
    [SerializeField] int clustersRequired;
    [SerializeField] Text textBox;
    private WorldManager wm;

    private void Start()
    {
        wm = FindObjectOfType<WorldManager>();
        textBox.text = "    x " + clustersRequired;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && wm.getFinalCrystals() >= clustersRequired)
        {
            Destroy(gameObject);
        }
    }
}
