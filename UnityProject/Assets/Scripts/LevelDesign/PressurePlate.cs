using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] int iceBlockLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == iceBlockLayer)
        {
            collision.transform.position = transform.position + (Vector3.up);
            collision.transform.rotation = transform.rotation;
            collision.gameObject.layer = 0; //For now, later may have player able to pick it up again.
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Destroy(door);
        }
    }
}
