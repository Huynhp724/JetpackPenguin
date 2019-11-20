using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherObject : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    private void OnDestroy()
    {
        Destroy(objectToDestroy);
    }
}
