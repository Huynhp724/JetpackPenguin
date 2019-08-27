using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public string nextLevelName;
    public string currentLevelName;

    ScreenFader fader;
    // Start is called before the first frame update
    void Start()
    {
        fader = GetComponent<ScreenFader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            NextLevel();
        }
    }


    public void NextLevel() {
        fader.Fade("out");
        StartCoroutine(LoadScene());

    }

    IEnumerator LoadScene() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextLevelName);
    }
}
