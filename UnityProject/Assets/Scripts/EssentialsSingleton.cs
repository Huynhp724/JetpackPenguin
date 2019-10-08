using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsSingleton : MonoBehaviour
{
    public static EssentialsSingleton essientialIsntace { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (essientialIsntace == null)
        {
            essientialIsntace = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        GameManager gm = GetComponentInChildren<GameManager>();
        if(gm == null)
        {
            Debug.Log("Game manager missing. Adding one.");
        }
    }


}
