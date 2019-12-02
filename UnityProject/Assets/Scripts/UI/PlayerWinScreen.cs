using Rewired;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWinScreen : MonoBehaviour
{
    [SerializeField] Animator winMenu;

    private PlayerController playerController;
    private ScreenManager sm;
    private Pause pauseController;

    // Start is called before the first frame update
    void Start()
    {
        sm = GetComponent<ScreenManager>();
        sm.ClosePanelBasic(winMenu.gameObject);
        playerController = FindObjectOfType<PlayerController>();
        pauseController = GetComponent<Pause>();
    }


    public void openWinScreen(float delayTime)
    {
        StartCoroutine(openScreen(delayTime));
    }

    IEnumerator openScreen(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        sm.OpenPanelBasic(winMenu.gameObject);
        playerController.enabled = false;
        playerController.GetComponent<Rigidbody>().isKinematic = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        pauseController.cantPause = true;
    }

    public void closeWinScreen()
    {
        sm.ClosePanelBasic(winMenu.gameObject);
        playerController.enabled = true;
        playerController.GetComponent<Rigidbody>().isKinematic = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        pauseController.cantPause = false;
    }
}
