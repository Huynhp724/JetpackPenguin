using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script handles the individual behavior of an ice bomb object.
public class SnowBomb : MonoBehaviour
{
    [SerializeField] GameObject snowplosion;
    private float gravityMultiplier = 2f;

    public void setGravityMultiplier(float gravMul)
    {
        gravityMultiplier = gravMul;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Instantiate(snowplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        float gravity = gravityMultiplier * Physics.gravity.y;
        GetComponent<Rigidbody>().AddForce(0, gravity, 0);
    }
}
