using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnerSystem : MonoBehaviour
{
    public SceneTransitionTrigger[] sceneTransitions;
    int entryScenePoint;

    GameObject player;

    private void Awake()
    {
        
    }


    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        entryScenePoint = LevelChanger.Instance.entryPoint;
        Debug.Log("Entry position is: " + entryScenePoint);

        for (int i = 0; i < sceneTransitions.Length; i++) {
            if (sceneTransitions[i].entryTarget == entryScenePoint) {
                Transform trans = sceneTransitions[i].transform.GetChild(0);
                player.transform.position = trans.position;
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
