using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string nextSceneName;
    public int entryTarget;

    int firstTime = 0;

    Vector3 reentryPosition;

    private void Awake()
    {
        reentryPosition = transform.GetChild(0).transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            LevelChanger.Instance.SetLevelInfo(nextSceneName, entryTarget);
        }
    }

    public void FirstTransition() {
        if (firstTime == 0)
        {
            LevelChanger.Instance.SetLevelInfo(nextSceneName, entryTarget);
            firstTime = 1;
        }
    }

    public void RestartLevel() {
        LevelChanger.Instance.SetLevelInfo(nextSceneName, entryTarget);
    }
}
