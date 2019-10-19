using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnVolume : MonoBehaviour
{

    public SceneTransitionTrigger[] triggers;

    int point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            point = LevelChanger.Instance.entryPoint;

            for (int i = 0; i < triggers.Length; i++) {
                if (triggers[i].entryTarget == point) {
                    Transform trans = triggers[i].transform.GetChild(0);
                    other.transform.position = trans.position;
                    break;
                }
            }
        }
    }
}
