using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public string currentLevelName;
    public Vector3 pos;
    public Vector3 newPos;
    public static LevelChanger Instance { get; private set; }


    public string previousLevelName = "", nextLevelName = "";
    GameObject player;
    bool needOldLocation = false;
    ScreenFader fader;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }

    void Start()
    {
        fader = GetComponent<ScreenFader>();
    }

    public void SetLevelInfo(string nxtLvl, Vector3 lastScenePosition) {
        nextLevelName = nxtLvl;
        

        if (previousLevelName == nxtLvl)
        {
            needOldLocation = true;
            Debug.Log("Next is the same as previous scene");
        }
        else {
            pos = lastScenePosition;
        }

        NextLevel();
    }


    public void NextLevel() {
        fader.Fade("out");
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene() {
        //pos = gameObject.transform.position;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextLevelName);
        previousLevelName = currentLevelName;
        currentLevelName = nextLevelName;
        nextLevelName = "";
        if (needOldLocation)
        {
            SetOldLocation();
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = newPos;
            Debug.Log("position has been set");
        }

        fader.Fade("in");

    }

    void SetOldLocation() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = pos;
        Debug.Log("position has been set");
    }
}
