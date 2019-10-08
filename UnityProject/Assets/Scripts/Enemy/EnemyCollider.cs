using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public float playerBounceBack;
    public GameObject headCollider;

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
            headCollider.SetActive(false);
            StartCoroutine(ToggleHeadCollider());
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            StartCoroutine(ToggleHeadCollider());
            PlayerController controller = collision.gameObject.GetComponentInParent<PlayerController>();
            if (controller.GetCurrentState() == PlayerController.State.Dashing)
            {
                PlayerAttackStats stats = collision.gameObject.GetComponentInParent<PlayerStats>().playerAttackStats;

                if (controller.getCurrentSlideSpeed() >= stats.maxSlideVelocity && !enemyStats.immuneToDash)
                {
                    enemyHealth.LoseLife();
                    return;
                }
            }

            


            PlayerHealth health = collision.gameObject.GetComponentInParent<PlayerHealth>();
            PlayerStats playerStats = collision.gameObject.GetComponentInParent<PlayerStats>();
            health.LoseHitpoint();

            Vector3 directionToPush = gameObject.transform.position - collision.gameObject.transform.position;
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
            playerRB.AddForce(directionToPush * -playerBounceBack, ForceMode.Impulse);
            Animator playerAnim = collision.gameObject.GetComponentInChildren<Animator>();

            //playerAnim.SetTrigger();
            controller.enabled = false;
            playerStats.TurnPlayerController(controller);
        }
    }


    IEnumerator ToggleHeadCollider() {
        yield return new WaitForSeconds(2f);
        headCollider.SetActive(true);
    }

}
