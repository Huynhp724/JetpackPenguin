using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour { 

    public int currentFish;
    public Text fishText;
    public Text winText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fishText.text = "Fish: " + currentFish + "/6";
        if (currentFish == 6)
        {
            winText.gameObject.SetActive(true);
        }
    }

    public void AddFish(int x)
    {
        currentFish += x;
    }
}
