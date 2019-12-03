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

    public delegate void UpdateTempUI(int lives, int purpleCrystals, int clusters);
    public event UpdateTempUI updateTempUI;

    public delegate void UpdateConstUI(float hpPercent, int crystals);
    public event UpdateConstUI updateConstUI;

    public delegate void UpdateOptions(float masterVol, float musicVol, float sfxVol, bool buttonPrompts);
    public event UpdateOptions updateOptions;

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
        updateAllUI();
        callGetOptions();
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
        if (worldStats.crystalsFound >= 100) {
            worldStats.crystalsFound = 0;
            addLives(1);
        }
        updateConstUI(getHealthPercent(), worldStats.crystalsFound);
    }

    public int getCrystals()
    {
        return worldStats.crystalsFound;
    }

    public void AddPurpleCrystal(int x)
    {
        Debug.Log("Adding " + x + " to Purple crystals.");
        worldStats.PurpleCrystals += x;
        updateAllUI();
    }

    public int getPurpleCrystals()
    {
        return worldStats.PurpleCrystals;
    }

    public void setCrystals(int x)
    {
        worldStats.crystalsFound = x;
        updateAllUI();
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

        updateAllUI();
    }

    public void resetHealth()
    {
        worldStats.playerHealth = worldStats.maxHealth;
        updateAllUI();
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
        updateAllUI();
    }

    //Can add or subtract if negative
    public void addLives(int lives)
    {
        worldStats.lives += lives;
        updateAllUI();
    }

    public void addFinalCrystal()
    {
        if(worldStats.finalCrystalsCollected <= 1)
        {
            worldStats.maxFuel += 25f;
        }
        else
        {
            worldStats.maxFuel += 50f;
        }
        worldStats.finalCrystalsCollected++;
        updateAllUI();
    }

    public int getFinalCrystals()
    {
        return worldStats.finalCrystalsCollected;
    }

    private void resetStats()
    {
        worldStats.crystalsFound = 0;
        worldStats.playerHealth = worldStats.maxHealth;
        worldStats.lives = worldStats.baseLives;
        worldStats.maxFuel = 0;
        worldStats.finalCrystalsCollected = 0;
        worldStats.levelDesignCollectablesTable = new Hashtable { };
        worldStats.PurpleCrystals = 0;
        //updateAllUI();
    }

    public bool checkCollected(string id) {

        if (worldStats.levelDesignCollectablesTable.Contains(id))
        {
            Debug.Log(worldStats.levelDesignCollectablesTable[id].ToString());
            return (bool)worldStats.levelDesignCollectablesTable[id];

        }
        else {
            worldStats.levelDesignCollectablesTable.Add(id, false);
            //Debug.Log("Adding colelctable " + id);
            return false;
        }
        
    }

    public void setCollected(string id) {

            worldStats.levelDesignCollectablesTable[id] = true;

    }

    public void updateAllUI()
    {
        try
        {
            updateTempUI(worldStats.lives, worldStats.PurpleCrystals, worldStats.finalCrystalsCollected);
            updateConstUI(getHealthPercent(), worldStats.crystalsFound);
        }
        catch
        {
            print("No UI to update. Likely in Menu Scene.");
        }
    }

    public void updateCrystals()
    {
        updateConstUI(getHealthPercent(), worldStats.crystalsFound);
    }

    public void callGetOptions()
    {
        updateOptions(worldStats.masterVolume, worldStats.musicVolume, worldStats.sfxVolume, worldStats.buttonPromptsOn);
    }

    public void setOptions(float masterVol, float musicVol, float sfxVol, bool buttonPrompts)
    {
        worldStats.masterVolume = masterVol;
        worldStats.musicVolume = musicVol;
        worldStats.sfxVolume = sfxVol;
        worldStats.buttonPromptsOn = buttonPrompts;
    }
}
