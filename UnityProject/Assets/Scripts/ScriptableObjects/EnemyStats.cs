using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public int hp;
    public float speed;
    public float rotSpeed;
    public float squishDetectionDistance;
    public int numOfSnowballsToFreeze;
    public bool immuneToDash = false;
    public bool immuneToSnowballs = false;
}
