using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Stat", menuName = "World Stats")]
public class WorldStats : ScriptableObject
{
    public int playerHealth;
    public int playerPoints;
    public float playerFuel;
    public int lives;
    public int maxHealth;
}
