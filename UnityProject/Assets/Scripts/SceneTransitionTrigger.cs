using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string nextSceneName;
    public int entryTarget;


    Vector3 reentryPosition;

    private void Awake()
    {
        reentryPosition = transform.GetChild(0).transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            LevelChanger.Instance.SetLevelInfo(nextSceneName, reentryPosition, entryTarget);
        }
    }

    public void FirstTransition() {
        LevelChanger.Instance.SetLevelInfo(nextSceneName, reentryPosition, entryTarget);
    }
}
