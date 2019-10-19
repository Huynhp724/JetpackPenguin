using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] WorldStats worldStats;
    //[SerializeField] PlayerAttackStats playerAttackStats;
    //Audio
    public float pitchShift = 1.0f;
    public bool shiftStart = false;
    public float shiftInterval = 10.0f;
    private float shiftIncrement = 0.0f;
    private float shiftVal = 0.0f;
    //For reset
    [SerializeField] bool resetOnAwake = false;
    

    private void Awake()
    {
        if(resetOnAwake)
        {
            resetStats();
        }
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

        //CHEAT CODES
        if (Input.GetKeyDown(KeyCode.Alpha7) && Input.GetKeyDown(KeyCode.Alpha8) && Input.GetKeyDown(KeyCode.Alpha9) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            worldStats.maxFuel += 50f;
        }
    }

    public float getMaxFuel()
    {
        return worldStats.maxFuel;
    }

    public void AddCrystal(int x)
    {
        Debug.Log("Adding " + x + " to crystals.");
        worldStats.crystalsFound += x;
        //crystalText.text = "Crystals: " + currentCrystals;
    }

    public int getCrystals()
    {
        return worldStats.crystalsFound;
    }

    public void setCrystals(int x)
    {
        worldStats.crystalsFound = x;
    }

    public int getHealthPoints()
    {
        return worldStats.playerHealth;
    }

    public void addHealthPoint(int hp)
    {
        if (worldStats.playerHealth + hp > worldStats.maxHealth)
            worldStats.playerHealth = worldStats.maxHealth;
        else
            worldStats.playerHealth += hp;
    }

    public void resetHealth()
    {
        worldStats.playerHealth = worldStats.maxHealth;
    }

    public float getHealthPercent()
    {
        float hpPercent = worldStats.playerHealth;
        return hpPercent / worldStats.maxHealth;
    }

    public float getLives()
    {
        return worldStats.lives;
    }

    public void resetLives()
    {
        worldStats.lives = worldStats.baseLives;
    }

    //Can add or subtract if negative
    public void addLives(int lives)
    {
        worldStats.lives += lives;
    }

    public void addFinalCrystal()
    {
        worldStats.maxFuel += 50f;
        worldStats.finalCrystalsCollected++;
    }

    private void resetStats()
    {
        worldStats.crystalsFound = 0;
        worldStats.playerHealth = worldStats.maxHealth;
        worldStats.lives = worldStats.baseLives;
        worldStats.maxFuel = 0;
        worldStats.finalCrystalsCollected = 0;
    }
}
