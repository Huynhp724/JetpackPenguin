using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour { 

    public int currentFish;
    public Text fishText;
    public Text winText;

    // TODO consider pulling out the the control mapping to its own control/settings manager.
    // Player controls mapping
    public string jump = "Jump";
    public string slide = "Slide";
    public string cameraX = "CameraX";
    public string cameraY = "CameraY";
    public string hover = "Hover";
    public string charge = "Charge";
    public float bumperThreshold = Mathf.Epsilon;

    // Start is called before the first frame update
    void Start()
    {
        string currentController = Input.GetJoystickNames()[1];
        print(currentController + " is connected.");

        if(currentController.Length == 19) //PS4 is named "Wireless Controller" = 19 chars
        {
            jump = "Jump_PS4";
            slide = "Slide_PS4";
            cameraX = "CameraX_PS4";
            cameraY = "CameraY_PS4";
            hover = "Hover_PS4";
            charge = "Charge_PS4";
            bumperThreshold = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fishText.text = "Fish: " + currentFish + "/6";
        if (currentFish == 6)
        {
            winText.gameObject.SetActive(true);
        }
    }

    public void AddFish(int x)
    {
        currentFish += x;
    }
}
