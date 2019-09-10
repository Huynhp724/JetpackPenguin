using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int lives;
    public int hitPoints;

    private static bool playerExists;

    bool isDead =false;

    public bool GetIsDead() {
        return isDead;
    }

    public void SetIsDead(bool dead) {
        isDead = dead;
    }

    private void Start()
    {
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

}
