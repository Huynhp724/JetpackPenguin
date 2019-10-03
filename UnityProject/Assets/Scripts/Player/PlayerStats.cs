using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerAttackStats playerAttackStats;
    public int lives;
    public int hitPoints;

    int setLives, setHitPoints;

    //private static bool playerExists;

    bool isDead =false;

    public bool GetIsDead() {
        return isDead;
    }

    public void SetIsDead(bool dead) {
        isDead = dead;
    }

    private void Start()
    {
        setLives = lives;
        setHitPoints = hitPoints;
    }

    public void ResetStats() {
        SetIsDead(false);
        lives = setLives;
        hitPoints = setHitPoints;
        
    }

    public float getHealthPercent()
    {
        float hpPercent = hitPoints;
        return hpPercent / setHitPoints;
    }

}
