using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string playerTag;
    public string nextSceneName;


    Vector3 reentryPosition;

    private void Awake()
    {
        reentryPosition = transform.GetChild(0).transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag)) {
            LevelChanger.Instance.SetLevelInfo(nextSceneName, reentryPosition);
        }
    }
}
