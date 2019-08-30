using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script handles the individual behavior of an ice bomb object.
public class SnowBomb : MonoBehaviour
{
    [SerializeField] GameObject snowplosion;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(snowplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
