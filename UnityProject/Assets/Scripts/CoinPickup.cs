using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float bobDegree = .75f;
    [SerializeField] float bobSpeed = 1f;
    [SerializeField] float pickedUpDistance = 2f;

    private bool pickedUp = false;

    private float intialY;

    // Start is called before the first frame update
    void Start()
    {
        intialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickedUp)
        {
            transform.Rotate(Vector3.up, rotateSpeed);
            transform.position = new Vector3(transform.position.x, intialY + (Mathf.Sin(Time.time * bobSpeed) * bobDegree), transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(PickUp(other.gameObject));
        }
    }

    IEnumerator PickUp(GameObject player)
    {
        pickedUp = true;
        FindObjectOfType<GameManager>().AddFish(1);
        transform.SetParent(player.transform);
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + pickedUpDistance, player.transform.position.z);
        yield return new WaitForSecondsRealtime(.5f);
        Destroy(gameObject);
    }
}
