using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowsplosion : MonoBehaviour
{
    [SerializeField] float explosionLength = 1f;

    void Awake()
    {
        Destroy(gameObject, explosionLength);
    }
}
