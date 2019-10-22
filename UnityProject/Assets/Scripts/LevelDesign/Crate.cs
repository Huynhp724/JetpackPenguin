using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject[] differentCollictables;
    public Animator[] bounceObjects;
    public float speedNeededToBreak;

    MeshRenderer mesh;
    Collider col;
    List<GameObject> newObjs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponentInParent<Rigidbody>();
            PlayerController playerCOntroller = collision.gameObject.GetComponentInParent<PlayerController>();

            if ((rb.velocity.magnitude >= speedNeededToBreak) && (playerCOntroller.GetCurrentState() == PlayerController.State.Dashing))
            {
                DestroyCrate();
            }
        }
    }

    public void DestroyCrate() {
        mesh.enabled = false;
        col.enabled = false;
        //int numOfSpawnedCollectables = Random.Range(0, 4);
        int numOfSpawnedCollectables = 4;
        int count = 0;
        
        while (numOfSpawnedCollectables > 0) {
            int choice = Random.Range(0, differentCollictables.Length);
            GameObject newObject = Instantiate(differentCollictables[choice], transform.position, transform.rotation);
            newObjs.Add(newObject);
            newObject.transform.parent = bounceObjects[count].gameObject.transform;
            bounceObjects[count].SetTrigger("Bounce");
            numOfSpawnedCollectables--;
            count++;
        }

        StartCoroutine(DeParentAndDestroy());

    }

    IEnumerator DeParentAndDestroy() {

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < newObjs.Count; i++) {
            newObjs[i].transform.parent = null;

        }

        Destroy(gameObject);
    }
}
