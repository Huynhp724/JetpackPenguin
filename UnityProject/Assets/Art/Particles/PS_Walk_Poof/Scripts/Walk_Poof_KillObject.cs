using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_Poof_KillObject : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.1f;
    private float timer;

    private void Awake()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
