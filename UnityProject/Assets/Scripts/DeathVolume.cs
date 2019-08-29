﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    public string playerTag;
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            health.LoseALife(true);
        }
        else {
            StartCoroutine(Destroy(other));
        }
    }

    IEnumerator Destroy(Collider other) {
        yield return new WaitForSeconds(destroyTime);
        Destroy(other.gameObject);
    }
}
