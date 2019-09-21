using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] Animator pauseMenu;

    private Player player;
    private ScreenManager sm;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        sm = GetComponent<ScreenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && player.GetButtonDown("Pause"))
        {
            paused = true;
            sm.OpenPanel(pauseMenu);
        }
        else if(paused && player.GetButtonDown("Pause"))
        {
            unpause();
        }
    }

    public void unpause()
    {
        paused = false;
        sm.CloseCurrent();
    }
}
