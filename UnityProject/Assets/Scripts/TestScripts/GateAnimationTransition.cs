using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAnimationTransition : MonoBehaviour
{

    bool firstTime = true;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstTime) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("FirstGateClose")) {
                anim.SetBool("FirstTime", false);
                firstTime = false;
            }
        }
    }
}
