using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour { 

    public int currentCrystals = 0;
    public int currentBigCrystals;
    public Text crystalText;
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

    private string currentController = "";
    private PlayerController playerControl;

    private void Start()
    {
        playerControl = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (string name in Input.GetJoystickNames())
        {
            if (name.Length > 1)
            {
                currentController = name;
            }
        }

        //print(currentController + " is connected.");

        if (currentController.Length == 19) //PS4 is named "Wireless Controller" = 19 chars
        {
            //Debug.Log("Using PS4 Controller");
            jump = "Jump_PS4";
            slide = "Slide_PS4";
            cameraX = "CameraX_PS4";
            cameraY = "CameraY_PS4";
            hover = "Hover_PS4";
            charge = "Charge_PS4";
            bumperThreshold = -1f;
        }
        //Then use xbox as default
        else
        {
            //Debug.Log("Using Xbox Controller");
            jump = "Jump";
            slide = "Slide";
            cameraX = "CameraX";
            cameraY = "CameraY";
            hover = "Hover";
            charge = "Charge";
}
        fishText.text = "Fish: " + currentFish;
        /*if (currentFish == 6)
        {
            winText.gameObject.SetActive(true);
        }*/
        //DEBUG cheat code to get more fuel. REMOVE from full builds.
        if(Input.GetKeyDown(KeyCode.Alpha7) && Input.GetKeyDown(KeyCode.Alpha8) && Input.GetKeyDown(KeyCode.Alpha9) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            AddBigCrystal();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKeyDown(KeyCode.Alpha5) && Input.GetKeyDown(KeyCode.Alpha6) && Input.GetKeyDown(KeyCode.Alpha7))
        {
            playerControl.transform.position = new Vector3(140, 0, 185);
        }
    }

    public void AddCrystal(int x)
    {
        currentCrystals += x;
        crystalText.text = "Crystals: " + currentCrystals;
    }

    public void AddBigCrystal()
    {
        currentBigCrystals++;
        playerControl.maxFuel += 50f;
    }
}
