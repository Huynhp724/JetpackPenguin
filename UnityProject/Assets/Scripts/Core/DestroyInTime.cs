using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    public float destroyInTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyInTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
