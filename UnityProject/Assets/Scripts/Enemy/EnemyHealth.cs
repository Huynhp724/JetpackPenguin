﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyStats stats;
    public Transform squishPosition;

    [HideInInspector] public bool isFreezing = false;

    [SerializeField] int hp;
    bool isDead;
    float maxTimer, timer;

    
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        hp = stats.hp;
        maxTimer = stats.freezeTimerPerHP;
        timer = stats.freezeTimerPerHP;
    }

    // Update is called once per frame
    void Update()
    {
        /*RaycastHit hit;
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

        }*/

        if (isFreezing) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                LoseHp();
                timer = maxTimer;
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

    public void LoseLife() {
        Destroy(gameObject);
    }

    public void SetFreeze(bool freeze) {
        isFreezing = freeze;

        if (!freeze) {
            timer = maxTimer;
        }
    }
}
