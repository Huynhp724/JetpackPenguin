using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade(string action) {
        if (action == "in")
        {
            anim.SetTrigger("fadeIn");
        }
        else if (action == "out") {

            anim.SetTrigger("fadeOut");
        }
    }
}
