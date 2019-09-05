using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Seal : MonoBehaviour
{
    public GameObject player;

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
        bTree = GetComponentInParent<BehaviorTree>();
        parentObject = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (keepTrackOfDistnace) {
            distnace = Vector3.Distance(player.transform.position, parentObject.position);
            if (distnace > 100f) {
                Debug.Log("Find Path");
                bTree.SendEvent("Find Path");
                keepTrackOfDistnace = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log("Chase");
            bTree.SendEvent("Chase");
            keepTrackOfDistnace = true;
        }
    }
}
