using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float lavaForcePush = 300;
    public GameObject mainCam;
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
        if (collision.gameObject.GetComponentInParent<PlayerHealth>() != null) {
            Debug.Log("Player fell into lava");
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            



            mainCam = collision.gameObject.transform.parent.GetChild(1).gameObject;
            mainCam.transform.Translate(Vector3.up * Time.deltaTime * lavaForcePush);
            rb.AddForce(Vector3.up * lavaForcePush, ForceMode.Impulse);
            //collision.gameObject.transform.Translate(Vector3.up * Time.deltaTime * lavaForcePush);

            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            playerHealth.LoseHitpoint();
        }
    }
}
