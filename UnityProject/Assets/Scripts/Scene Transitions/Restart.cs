using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public Pause pause;


    public void RestartClick() {
        pause.unpause();
    }
}
