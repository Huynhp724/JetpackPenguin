using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutScene : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CutSceneCamera[] cameras;
    [SerializeField] CinemachineBrain cmBrain;
    [SerializeField] float timeBackToPlayer = 2;
    private PlayerController pc;
    private PlayerAbilities pa;

    private int OFF = 5;
    private int ON = 15;

    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerController>();
        pa = player.GetComponent<PlayerAbilities>();
        pc.enabled = false;
        pa.enabled = false;
        StartCoroutine(NextCamera(0));
    }

    IEnumerator NextCamera(int index)
    {
        print("Camera: " + index);   

        yield return new WaitForSecondsRealtime(cmBrain.m_DefaultBlend.m_Time + cameras[index].timeToWait);

        cameras[index].setPriority(OFF);

        if (index + 1 < cameras.Length)
        {
            cmBrain.m_DefaultBlend.m_Time = cameras[index + 1].timeToGetHere;
            cameras[index + 1].setPriority(ON);
            StartCoroutine(NextCamera(index + 1));
        }
        else
        {
            cmBrain.m_DefaultBlend.m_Time = timeBackToPlayer;
            pc.enabled = true;
            pa.enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
