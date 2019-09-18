using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    EnemyHealth enemyHealth;
    EnemyStats enemyStats;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyStats = GetComponent<EnemyHealth>().stats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerController controller = other.GetComponentInParent<PlayerController>();
            if (controller.GetCurrentState() == PlayerController.State.Dashing)
            {
                PlayerAttackStats stats = other.GetComponentInParent<PlayerStats>().playerAttackStats;
                //Debug.Log("Hit while dashing");
                Debug.Log(controller.getCurrentSlideSpeed());
                if (controller.getCurrentSlideSpeed() >= stats.maxSlideVelocity && !enemyStats.immuneToDash)
                {
                    enemyHealth.LoseLife();
                    return;
                }
            }


            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            health.LoseHitpoint();
        }

    }

}
