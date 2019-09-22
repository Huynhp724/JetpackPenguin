using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float bobDegree = .75f;

    private float intialY;

    // Start is called before the first frame update
    void Start()
    {
        intialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed);
        transform.position = new Vector3(transform.position.x, intialY + (Mathf.Sin(Time.time) * bobDegree), transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<GameManager>().AddFish(1);
            Destroy(gameObject);
        }
    }
}
