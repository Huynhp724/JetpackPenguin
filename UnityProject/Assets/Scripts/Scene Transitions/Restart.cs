using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    LevelChanger lvlChanger;
    // Start is called before the first frame update
    void Start()
    {
        lvlChanger = LevelChanger.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel() {
        SceneManager.LoadScene(lvlChanger.GetCurrentLevelName());
    }
}
