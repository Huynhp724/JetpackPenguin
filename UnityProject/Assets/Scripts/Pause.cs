using Rewired;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] Animator pauseMenu;
    [SerializeField] Animator optionsMenu;

    private PlayerController playerController;
    private CinemachineFreeLook cam;

    private Player player;
    private ScreenManager sm;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        sm = GetComponent<ScreenManager>();
        sm.ClosePanelBasic(pauseMenu.gameObject);
        playerController = FindObjectOfType<PlayerController>();
        cam = FindObjectOfType<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && player.GetButtonDown("Pause"))
        {
            pause();
        }
        else if(paused && player.GetButtonDown("Pause"))
        {
            unpause();
        }
    }

    public void pause()
    {
        paused = true;
        sm.OpenPanelBasic(pauseMenu.gameObject);
        playerController.enabled = false;
        playerController.GetComponent<Rigidbody>().isKinematic = true;
        cam.enabled = false;
        Time.timeScale = 0;
    }

    public void unpause()
    {
        paused = false;
        sm.ClosePanelBasic(pauseMenu.gameObject);
        sm.ClosePanelBasic(optionsMenu.gameObject);
        playerController.enabled = true;
        playerController.GetComponent<Rigidbody>().isKinematic = false;
        cam.enabled = true;
        Time.timeScale = 1;
    }
}
