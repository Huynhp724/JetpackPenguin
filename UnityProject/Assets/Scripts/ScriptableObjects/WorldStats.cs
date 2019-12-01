using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Stat", menuName = "World Stats")]
public class WorldStats : ScriptableObject
{
    public int playerHealth;
    public int crystalsFound;
    public int PurpleCrystals;
    public float maxFuel;
    public int lives;
    public int baseLives;
    public int maxHealth;
    public Hashtable levelDesignCollectablesTable;
    public int finalCrystalsCollected;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public bool buttonPromptsOn;
}
