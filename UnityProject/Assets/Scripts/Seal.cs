using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public enum SealType { BABY, FAT, SHOOTER, BOSS };

public class Seal : MonoBehaviour
{
    GameObject player;
    public float goBackToPathAfterDistance;
    public SealType sealType;

    bool keepTrackOfDistnace = false, charge;
    float distnace;
    Transform parentObject;
    BehaviorTree bTree;
    EnemyStats stats;

    public GameObject GetPlayer() {
        return player;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bTree = GetComponentInParent<BehaviorTree>();
        stats = GetComponentInParent<EnemyHealth>().stats;
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

        if (charge) {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, stats.speed * 2 * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            bTree.SendEvent("Chase");
            keepTrackOfDistnace = true;

            if (sealType == SealType.FAT)
            {
                StartCoroutine(ChargeTimer());
            }
        }
    }

    public void Charge() {
        //Vector3.Lerp(transform.position, player.transform.position, stats.speed * Time.deltaTime);
        bTree.SendEvent("Charge");
    }

    IEnumerator ChargeTimer() {
        Charge();
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(ChargeTimer());
    }
}
