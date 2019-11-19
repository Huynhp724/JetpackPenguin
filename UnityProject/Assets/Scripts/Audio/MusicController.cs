using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip penguinVilalgeTheme;
    public AudioClip levelTheme;
    
    // Start is called before the first frame update
    void Start()
    {
        Scene current = SceneManager.GetActiveScene();
        string scenename = current.name;
        if (scenename == "LevelDesign" || scenename == "Level Design Art Pass")
        {
            aud.clip = penguinVilalgeTheme;
        }
        else {
            aud.clip = levelTheme;
        }
        aud.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
