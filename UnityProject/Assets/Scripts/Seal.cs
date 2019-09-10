using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal : MonoBehaviour
{
    public GameObject player;
    public Animator fsmAnimator;

    bool keepTrackOfDistnace = false;
    float distnace;

    public GameObject GetPlayer() {
        return player;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keepTrackOfDistnace) {
            distnace = Vector3.Distance(player.transform.position, fsmAnimator.gameObject.transform.position);
            fsmAnimator.SetFloat("distance", distnace);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            fsmAnimator.SetTrigger("chase");
            keepTrackOfDistnace = true;
        }
    }
}
