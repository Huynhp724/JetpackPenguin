using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutSceneCamera : MonoBehaviour
{
    CinemachineVirtualCamera vc;
    public float timeToWait = 1;
    public float timeToGetHere = 2;
    public bool noWait = false;

    private void Start()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
    }

    public void setPriority(int x)
    {
        vc.Priority = x;
    }
}
