using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Seal : MonoBehaviour
{
    GameObject player;
    public float goBackToPathAfterDistance;

    bool keepTrackOfDistnace = false;
    float distnace;
    Transform parentObject;
    BehaviorTree bTree;

    public GameObject GetPlayer() {
        return player;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bTree = GetComponentInParent<BehaviorTree>();
        parentObject = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (keepTrackOfDistnace) {
            distnace = Vector3.Distance(player.transform.position, parentObject.position);
            if (distnace > goBackToPathAfterDistance) {
                Debug.Log("Find Path");
                bTree.SendEvent("Find Path");
                keepTrackOfDistnace = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            bTree.SendEvent("Chase");
            keepTrackOfDistnace = true;
        }
    }
}
