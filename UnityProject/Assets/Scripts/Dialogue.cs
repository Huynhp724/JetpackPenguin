using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Place on main character of an NPC
// Sets the name of the npc, the priority of each individual dialogue option and has multiple strings to place dialogue.

public class Dialogue : MonoBehaviour
{
    public string name;                     // name of character
    public int priority;                    // priority of dialogue used to choose which one to say

    [TextArea(3, 10)]
    public string[] sentences;              // the dialoge the character will say
}
