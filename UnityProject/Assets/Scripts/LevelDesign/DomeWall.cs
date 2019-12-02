using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeWall : MonoBehaviour
{
    [SerializeField] PlayerWinScreen winScreen;
    [SerializeField] float delayTillWin = 1f;
    [SerializeField] GameObject globeBreakParticles;

    private void Start()
    {
        if (winScreen == null)
            winScreen = FindObjectOfType<PlayerWinScreen>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
        if(pc && (pc.getChargedJumped()))
        {
            winScreen.openWinScreen(delayTillWin);
            Instantiate(globeBreakParticles, transform.parent.position, transform.rotation);
            GetComponentInParent<Dome>().destroyDome();
        }
    }
}
