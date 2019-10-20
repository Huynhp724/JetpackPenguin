using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Animator animator;
    private PlayerController pc;
    private PlayerAbilities pa;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        pc = player.GetComponent<PlayerController>();
        pa = player.GetComponent<PlayerAbilities>();
        pc.enabled = false;
        pa.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            pc.enabled = true;
            pa.enabled = true;
        }
    }
}
