using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] float playerSpeedThreshhold = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerControl = other.gameObject.GetComponent<PlayerController>();
        if (playerControl)
        {
            //float angle = Mathf.Atan2(playerControl.controller.velocity.z, playerControl.controller.velocity.x);
            //float playerSpeed = Mathf.Sqrt(Mathf.Pow(playerControl.controller.velocity.x, 2) + Mathf.Pow(playerControl.controller.velocity.z, 2));
            float playerSpeed = playerControl.getCurrentSlideSpeed();
            print(playerSpeed);
            if (playerSpeed > playerSpeedThreshhold)
            {
                Destroy(gameObject);
            }
        }
    }
}
