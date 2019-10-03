using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealBaseFSM : StateMachineBehaviour
{
    public GameObject player;
    public GameObject npc;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc = animator.gameObject;
        player = npc.GetComponent<Seal>().GetPlayer();

    }
}
