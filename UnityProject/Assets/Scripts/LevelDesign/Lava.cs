using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float lavaForcePush = 300;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null) {
            Debug.Log("Player fell into lava");
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * lavaForcePush, ForceMode.Impulse);

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.LoseHitpoint();
        }
    }
}
