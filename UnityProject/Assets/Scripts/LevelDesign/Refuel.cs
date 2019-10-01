using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refuel : MonoBehaviour
{
    public GameObject Player;
    public float thrust;

    private bool inGeyser;
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(inGeyser == true)
        {
            Player.GetComponent<PlayerController>().currentFuel = 500;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inGeyser = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inGeyser = false;
        }
    }
}
