using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackFreeze : MonoBehaviour
{
    public GameObject raycastStartPosiiton;
    public GameObject laveFreezeBlocks;

    bool canShootRaycast = true;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            enemyHealth.SetFreeze(true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Lava")) {
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
        if (canShootRaycast)
        {
            canShootRaycast = false;
            StartCoroutine(Raycast());
        }
        
    }

    IEnumerator Raycast() {
        yield return new WaitForSeconds(0.1f);

        Ray ray = new Ray(raycastStartPosiiton.transform.position, Vector3.down);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Instantiate(laveFreezeBlocks, hit.point, laveFreezeBlocks.transform.rotation);
        }

        canShootRaycast = true;
    }



}
