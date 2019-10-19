using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowsplosion : MonoBehaviour
{
    [SerializeField] float explosionLength = 1f;
    public LayerMask iceBombEffectLayer;

    Collider[] allColliders;

    void Awake()
    {
        Destroy(gameObject, explosionLength);
        DoSphereCast();
    }

    void DoSphereCast() {
        allColliders = Physics.OverlapSphere(transform.position, 3f, iceBombEffectLayer);
        for (int i = 0; i < allColliders.Length; i++) {
            if (allColliders[i].CompareTag("Head")) {
                Debug.Log("Freeze bombing " + allColliders[i].name);
                EnemyHealth enemyHealth = allColliders[i].GetComponentInParent<EnemyHealth>();
                if (!enemyHealth.stats.immuneToSnowballs) {
                    enemyHealth.CreateIceBlock();
                    Destroy(enemyHealth.gameObject);
                }
                break;
            }
        }
    }
}
