using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public WorldStats worldStats;
    //Player
    public GameObject Player;
    public PlayerAttackStats playerAttackStats;
    public GameObject maincam;
    //Audio
    public float pitchShift = 1.0f;
    public bool shiftStart = false;
    public float shiftInterval = 10.0f;
    private float shiftIncrement = 0.0f;
    private float shiftVal = 0.0f;
    //Crystals
    public int currentCrystals;
    //Health
    public int lives;
    public int hitPoints;
    public int maxHitPoints;
    

    private void Awake()
    {
        LoadStats();
    }

    // Update is called once per frame
    void Update()
    {
        shiftVal += shiftIncrement;
        if (shiftStart && shiftIncrement != 0.0f)
        {
            pitchShift += 0.1f;
            shiftVal = 0.0f;
            shiftStart = false;
        }
        else if (shiftStart && shiftIncrement == 0.0f)
        {
            shiftIncrement = 0.1f;
        }
        else if (shiftVal > shiftInterval)
        {
            shiftVal = 0.0f;
            pitchShift = 1.0f;
            shiftStart = false;
            shiftIncrement = 0.0f;
        }
    }

    public void AddCrystal(int x)
    {
        Debug.Log("Adding " + x + " to crystals.");
        currentCrystals += x;
        //crystalText.text = "Crystals: " + currentCrystals;
    }

    public int getCrystals()
    {
        return currentCrystals;
    }

    public float getHealthPercent()
    {
        float hpPercent = hitPoints;
        return hpPercent / maxHitPoints;
    }

    public void LoadStats()
    {
        PlayerStats stats = Player.GetComponent<PlayerStats>();
        stats.lives = worldStats.lives;
        stats.hitPoints = worldStats.playerHealth;
        currentCrystals = worldStats.playerPoints;
    }

    public void SaveStats() {
        PlayerStats stats = Player.GetComponent<PlayerStats>();
        worldStats.lives = stats.lives;
        worldStats.playerHealth = stats.hitPoints;
        worldStats.playerPoints = currentCrystals;
    }


}
