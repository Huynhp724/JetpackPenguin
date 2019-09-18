using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player Attack Stats")]
public class PlayerAttackStats : ScriptableObject
{
    public float maxSlideVelocity;
    public float slideDamage;
    public float hoverFreezeDamage;
    public int iceBombDamage;
}
