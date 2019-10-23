using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutScene : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CutSceneCamera[] cameras;
    [SerializeField] CinemachineBrain cmBrain;
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
        //cmBrain.m_DefaultBlend.m_Time = 5;
        while(cmBrain.IsBlending)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(cameras[index].timeToWait);

        cameras[index].setPriority(OFF);

        if (index + 1 < cameras.Length)
        {
            cameras[index + 1].setPriority(ON);
            StartCoroutine(NextCamera(index + 1));
        }
        else
        {
            pc.enabled = true;
            pa.enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
