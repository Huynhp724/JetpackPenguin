using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMagnet : MonoBehaviour
{
    private bool tracking;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        tracking = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        tracking = true;
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        player = playerHealth.gameObject;
        GetComponentInParent<CoinPickup>().magnet = true;
        GetComponentInParent<CoinPickup>().magnetTarget = player;

    }
}
