using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refuel : MonoBehaviour
{
    public GameObject Player;
    public GameObject ring;
    public float thrust;

    private bool contact;
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        ring.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (contact == true)
        {
            Player.GetComponent<PlayerController>().currentFuel = Player.GetComponent<PlayerController>().maxFuel;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = false;
            ring.SetActive(false);
            StartCoroutine(Respawn(30.0f));
        }
    }

    private IEnumerator Respawn(float timer)
    {
        yield return new WaitForSeconds(timer);
        ring.SetActive(true);
    }

}
