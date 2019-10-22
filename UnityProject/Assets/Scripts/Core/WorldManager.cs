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

    public delegate void UpdateUI(float hpPercent, int crystals, int lives); // declares new delegate type
    public event UpdateUI updateUI;

    private void Awake()
    {
        if(resetOnAwake)
        {
            resetStats();
        }
        if (worldStats.levelDesignCollectablesTable == null)
        {
            worldStats.levelDesignCollectablesTable = new Hashtable { };
        }
    }

    private void Start()
    {
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
        
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
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
    }

    public int getCrystals()
    {
        return worldStats.crystalsFound;
    }

    public void setCrystals(int x)
    {
        worldStats.crystalsFound = x;
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
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

        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
    }

    public void resetHealth()
    {
        worldStats.playerHealth = worldStats.maxHealth;
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
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
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
    }

    //Can add or subtract if negative
    public void addLives(int lives)
    {
        worldStats.lives += lives;
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
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
        worldStats.levelDesignCollectablesTable = null;
        updateUI(getHealthPercent(), worldStats.crystalsFound, worldStats.lives);
    }

    public bool checkCollected(string id) {

        if (worldStats.levelDesignCollectablesTable.Contains(id))
        {
            Debug.Log(worldStats.levelDesignCollectablesTable[id].ToString());
            return (bool)worldStats.levelDesignCollectablesTable[id];

        }
        else {
            worldStats.levelDesignCollectablesTable.Add(id, false);
            Debug.Log("Adding colelctable " + id);
            return false;
        }
        
    }

    public void setCollected(string id) {

            worldStats.levelDesignCollectablesTable[id] = true;

    }
}
