﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Slider fuelGauge;
    [SerializeField] Slider healthBar;
    [SerializeField] Text lives;
    private PlayerController playerControl;
    private PlayerStats playerStats;


    // Start is called before the first frame update
    void Start()
    {
        playerControl = FindObjectOfType<PlayerController>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        fuelGauge.value = playerControl.currentFuel / playerControl.maxFuel;
        healthBar.value = playerStats.getHealthPercent();
        lives.text = "Lives: " + playerStats.lives;
    }
}