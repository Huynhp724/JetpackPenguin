using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Slider fuelGauge;
    [SerializeField] Image healthWheel;
    [SerializeField] Text lives;
    [SerializeField] Text crystalText;
    [SerializeField] Text purpleCrystalText;
    [SerializeField] Transform crystal;
    [SerializeField] Transform purpleCrystal;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] WorldManager wm; 
    private PlayerController playerControl;


    // Start is called before the first frame update
    void Start()
    {
        if (wm == null)
        {
            wm = FindObjectOfType<WorldManager>();
        }
        playerControl = FindObjectOfType<PlayerController>();
        wm.updateUI += OnUpdateUI;
    }

    void OnUpdateUI(float hpPercent, int crystals, int life, int purpleCrystals)
    {
        healthWheel.fillAmount = hpPercent;
        lives.text = "Lives: " + life;
        crystalText.text = "x " + crystals;
        purpleCrystalText.text = "x " + purpleCrystals;
    }

    // Update is called once per frame
    void Update()
    {
        fuelGauge.value = playerControl.currentFuel / playerControl.maxFuel;
        crystal.Rotate(Vector3.up, rotateSpeed);
        purpleCrystal.Rotate(Vector3.up, rotateSpeed);
    }
}
