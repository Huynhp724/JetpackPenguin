using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackFreeze : MonoBehaviour
{
    public GameObject raycastStartPosiiton;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            enemyHealth.SetFreeze(true);
        }
        else if (other.CompareTag("Lava")) {
            ShootRaycast();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Head")) {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            enemyHealth.SetFreeze(false);
        }
    }

    void ShootRaycast() {

        Ray ray = new Ray(raycastStartPosiiton.transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

       // if(Physics.Raycast(ray, 3f, out hit))
        
    }



}
