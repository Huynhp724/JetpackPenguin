using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
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
            //Player.GetComponent<Transform>().Translate(Vector3.up * Time.deltaTime * thrust, Space.World);
            Player.GetComponent<Rigidbody>().useGravity = false;
            //Player.GetComponent<Rigidbody>().AddForce(transform.up * thrust);
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
