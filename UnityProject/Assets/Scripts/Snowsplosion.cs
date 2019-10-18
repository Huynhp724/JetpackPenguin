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
            Debug.Log("name " +allColliders[i].name);
            if (allColliders[i].CompareTag("Head") && (allColliders[i].GetComponent<EnemyHealth>() != null))
            {
                
                EnemyHealth enemyHealth = allColliders[i].GetComponentInParent<EnemyHealth>();
                if (!enemyHealth.stats.immuneToSnowballs)
                {
                    enemyHealth.CreateIceBlock();
                    Destroy(enemyHealth.gameObject);
                }
                break;
            }
            else if (allColliders[i].CompareTag("Crate")) {
                allColliders[i].GetComponent<Crate>().DestroyCrate();
            } else if (allColliders[i].gameObject.layer == LayerMask.NameToLayer("Waterfall")) {
                Debug.Log("Got waterfall");
                allColliders[i].GetComponent<Waterfall>().ChangeForm(false);
            }
        }
    }
}
