using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int lives;
    public int hitPoints;

    bool isDead =false;

    public bool GetIsDead() {
        return isDead;
    }

    public void SetIsDead(bool dead) {
        isDead = dead;
    }

}
