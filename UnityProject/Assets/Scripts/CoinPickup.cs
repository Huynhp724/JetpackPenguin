using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pickup {LIFEHEALTH, CRYSTAL, HITPOINTHEALTH, COUNT };

public class CoinPickup : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float bobDegree = .75f;
    [SerializeField] float bobSpeed = 1f;
    [SerializeField] float pickedUpDistance = 2f;

    [SerializeField] AudioClip pickupSound;
    public Pickup pickupEnum = Pickup.CRYSTAL;

    private bool pickedUp = false;

    private float intialY;
    private GameManager gm;
    private AudioSource asource;
    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
        intialY = transform.position.y;
       // gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickedUp)
        {
            transform.Rotate(Vector3.up, rotateSpeed);
            transform.position = new Vector3(transform.position.x, intialY + (Mathf.Sin(Time.time * bobSpeed) * bobDegree), transform.position.z);
        }

       // if(gm == null)
       // {
      //      gm = FindObjectOfType<GameManager>();
      //  }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collectable triggered. Collided with: " + other.gameObject);
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth && !pickedUp)
        {
            Debug.Log("It is player.");
            if (pickupEnum == Pickup.LIFEHEALTH)
            {
                playerHealth.GainLife();
            }
            else if (pickupEnum == Pickup.HITPOINTHEALTH)
            {
                playerHealth.GainHitpoint();
            }
            else if (pickupEnum == Pickup.CRYSTAL) {
                Debug.Log("It is a crystal.");
                stats = playerHealth.GetComponent<PlayerStats>();
                stats.AddCrystal(1);
            }

            StartCoroutine(PickUp(playerHealth.gameObject));
        }
    }

    IEnumerator PickUp(GameObject player)
    {
        Debug.Log("Pick up coroutine");
        pickedUp = true;
        stats.shiftStart = true;
        asource.pitch = stats.pitchShift;
        asource.volume = 0.7f;
        asource.PlayOneShot(pickupSound);
        transform.SetParent(player.transform);
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + pickedUpDistance, player.transform.position.z);
        yield return new WaitForSecondsRealtime(.5f);
        Destroy(gameObject);
    }
}
