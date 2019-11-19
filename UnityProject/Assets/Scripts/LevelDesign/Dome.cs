using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    [SerializeField] float forceMagnitude = 25f;
    [SerializeField] float radius = 10f;
    [SerializeField] Transform center;

    private WorldManager wm;
    private string id;
    private Rigidbody playerRb;

    void Start()
    {
        id = transform.position.ToString();
        wm = FindObjectOfType<WorldManager>();
        if (wm.checkCollected(id))
        {
            Destroy(gameObject);
        }
    }

    public void destroyDome()
    {
        wm.setCollected(id);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if(playerRb != null && Vector3.Distance(center.position, playerRb.transform.position) > radius)
        {
            Vector3 directionToCenter = (center.position - playerRb.transform.position).normalized;
            Vector3 f = directionToCenter * forceMagnitude;
            playerRb.AddForce(f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(playerRb == null && other.CompareTag("Player"))
        {
            playerRb = other.GetComponentInParent<Rigidbody>();
        }

    }
}
