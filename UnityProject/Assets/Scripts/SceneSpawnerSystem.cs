using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnerSystem : MonoBehaviour
{
    public SceneTransitionTrigger[] sceneTransitions;
    int entryScenePoint;

    public GameObject player;
    GameObject mainCam;
    GameObject cmFreeLook;



    private void OnEnable()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
       // mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        //cmFreeLook = GameObject.FindGameObjectWithTag("CMFreeLook");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        entryScenePoint = LevelChanger.Instance.entryPoint;
        Debug.Log("Entry position is: " + entryScenePoint);

        for (int i = 0; i < sceneTransitions.Length; i++) {
            if (sceneTransitions[i].entryTarget == entryScenePoint) {
                /*Transform trans = sceneTransitions[i].transform.GetChild(0);
                player.transform.position = trans.position;
                cmFreeLook.transform.position = trans.position;
                mainCam.transform.position = trans.position;
                break;*/

                player.transform.position = sceneTransitions[i].transform.GetChild(0).position;
                
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
