using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyStats stats;
    public Transform squishPosition;

    int hp;
    bool isDead;

    
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        hp = stats.hp;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        ray = new Ray(squishPosition.position, Vector3.up * stats.squishDetectionDistance);
        Debug.DrawRay(squishPosition.position, Vector3.up * stats.squishDetectionDistance, Color.cyan);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Player"))
            {
                PlayerHealth health = hit.collider.GetComponentInParent<PlayerHealth>();
                health.SetInvincibilty(true);
                LoseHp();

            }

        }
    }

    public void LoseHp() {
        if (!isDead) {
            if (hp > 1)
            {
                hp -= 1;
            }
            else {
                hp = 0;
                isDead = true;
                Debug.Log("Squished");
                Destroy(gameObject);
            }
        }
    }
}
