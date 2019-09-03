using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSealFSM : StateMachineBehaviour
{
    public GameObject npc;
    public GameObject player;

    public float speed = 3.5f;
    public float rotationSpeed = 2.5f;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc = animator.gameObject;

        player = npc.GetComponentInChildren<Seal>().GetPlayer();
    }
}
