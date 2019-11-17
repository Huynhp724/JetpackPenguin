using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
        if(pc && (pc.getChargedJumped()))
        {
            GetComponentInParent<Dome>().destroyDome();
        }
    }
}
