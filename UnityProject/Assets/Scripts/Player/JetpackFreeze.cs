using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackFreeze : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head")) {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            enemyHealth.SetFreeze(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Head")) {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            enemyHealth.SetFreeze(false);
        }
    }

}
