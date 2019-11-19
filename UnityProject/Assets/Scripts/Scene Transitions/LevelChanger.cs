using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    string currentLevelName;
    public Vector3 pos;
    public Vector3 newPos;
    //public Camera mainCam;
    public static LevelChanger Instance { get; private set; }


    public string nextLevelName = "";
    public int entryPoint;
    bool needOldLocation = false;
    bool startFade = false;
    ScreenFader fader;
    GameObject player;

    public float fadeTime = 1f;

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

    private void Update()
    {

    }

    void Start()
    {
        fader = GetComponent<ScreenFader>();
    }

    public void SetLevelInfo(string nxtLvl, Vector3 lastScenePosition, int entryValue) {
        nextLevelName = nxtLvl;
        entryPoint = entryValue;
        Debug.Log("Entry point is: " + entryValue);
        StartCoroutine(WaitToFadeOut());
        //NextLevel();
    }


    public void NextLevel() {
        //fader.Fade("out");
       // GameObject player = GameObject.FindGameObjectWithTag("Player");
        //GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        //Destroy(player);
        //StartCoroutine(LoadScene());
        StartCoroutine(LoadScenePlacePluck());
    }

    IEnumerator LoadScene() {
        //pos = gameObject.transform.position;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextLevelName);

        FindLocation();

        fader.Fade("in");

    }

    IEnumerator LoadScenePlacePluck() {
        SceneManager.LoadScene(nextLevelName);

        yield return new WaitForSeconds(2f);
        player = GameObject.Find("Player");
        currentLevelName = nextLevelName;

        StartCoroutine(WaitToFadeIn());




        //FindLocation();



    }

    IEnumerator WaitToFadeOut() {
        fader.Fade("out");
        yield return new WaitForSeconds(fadeTime);
        NextLevel();
        
    }

    IEnumerator WaitToFadeIn()
    {
        yield return new WaitForSeconds(fadeTime);
        fader.Fade("in");

    }

    void FindLocation() {
        GameObject[] entryDoors = GameObject.FindGameObjectsWithTag("EntryPoint");
        foreach (GameObject obj in entryDoors) {
            SceneTransitionTrigger trigger = obj.GetComponent<SceneTransitionTrigger>();
            Debug.Log(obj.name + " " + obj.transform.localPosition);
            if (trigger.entryTarget == entryPoint) {
                Transform transtionTranform = obj.transform.GetChild(0);
                Debug.Log(obj.transform.localPosition);
                player = GameObject.FindGameObjectWithTag("Player");
                player.transform.localPosition = transtionTranform.position;
                //GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
                //Camera.transform.localPosition = transtionTranform.position;
                
            }
        }
    }

    public string GetCurrentLevelName() {
        return currentLevelName;
    }
}
