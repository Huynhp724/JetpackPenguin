using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Place on Main canvas in the scene
// Deals with the UI in the game.

public class UIController : MonoBehaviour
{
    public GameObject speakingButton;                     // button that is used to speak with characters

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeakingInteractionButton(bool on) {
        speakingButton.SetActive(on);
    }
}
