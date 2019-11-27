﻿using System.Collections;
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
    [SerializeField] Text clusterText;
    [SerializeField] Transform crystal;
    [SerializeField] Transform purpleCrystal;
    [SerializeField] Transform cluster;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] WorldManager wm;
    [SerializeField] float displayElementsDelay = 2f;
    [SerializeField] GameObject iceBlockPrompts;
    [SerializeField] GameObject buttonPrompts;
    [SerializeField] Text yText;
    [SerializeField] Text xText;
    private PlayerController playerControl;
    private PlayerAbilities PlayerAbilities;
    private float timeSinceMoved;
    private bool showElements = false;
    private string tempXText = "";
    private string tempYText = "";
    private bool isDisplayingIceBlockPrompts = false;
    


    // Start is called before the first frame update
    void Start()
    {
        if (wm == null)
        {
            wm = FindObjectOfType<WorldManager>();
        }
        playerControl = FindObjectOfType<PlayerController>();
        PlayerAbilities = playerControl.GetComponent<PlayerAbilities>();
        wm.updateTempUI += OnUpdateTempUI;
        wm.updateConstUI += OnUpdateConstUI;
        wm.updateAllUI();
        wm.updateCrystals();
    }

    void OnUpdateTempUI(int life, int purpleCrystals, int clusters)
    {
        lives.text = "Lives: " + life;
        purpleCrystalText.text = "x " + purpleCrystals;
        clusterText.text = "x " + clusters;
        StartCoroutine(showTempElements());
    }

    void OnUpdateConstUI(float hpPercent, int crystals)
    {
        healthWheel.fillAmount = hpPercent;
        crystalText.text = "x " + crystals;
    }

    // Update is called once per frame
    void Update()
    {
        fuelGauge.value = playerControl.currentFuel / playerControl.maxFuel;
        crystal.Rotate(Vector3.up, rotateSpeed);
        purpleCrystal.Rotate(Vector3.up, rotateSpeed);
        cluster.Rotate(Vector3.up, rotateSpeed);
        if (!showElements && (Mathf.Abs(playerControl.getHoriInput()) > 0 || Mathf.Abs(playerControl.getVertInput()) > 0))
        {
            hidePurpleCystals();
            hideClusters();
            hideLives();
            timeSinceMoved = Time.time;
        }
        else if(showElements || timeSinceMoved + displayElementsDelay < Time.time)
        {
            showPurpleCystals();
            showClusters();
            showLives();
        }

        if(!isDisplayingIceBlockPrompts && PlayerAbilities.heldIceBlock != null)
        {
            showButtonPrompts();
        }
        else if(isDisplayingIceBlockPrompts && PlayerAbilities.heldIceBlock == null)
        {
            hideButtonPrompts();
        }
    }

    //TODO: Have these animate or lerp in and out of the screen.
    void showPurpleCystals()
    {
        purpleCrystal.gameObject.SetActive(true);
        purpleCrystalText.gameObject.SetActive(true);
    }

    void hidePurpleCystals()
    {
        purpleCrystal.gameObject.SetActive(false);
        purpleCrystalText.gameObject.SetActive(false);
    }

    void showClusters()
    {
        cluster.gameObject.SetActive(true);
        clusterText.gameObject.SetActive(true);
    }

    void hideClusters()
    {
        cluster.gameObject.SetActive(false);
        clusterText.gameObject.SetActive(false);
    }

    void showLives()
    {
        lives.gameObject.SetActive(true);
    }

    void hideLives()
    {
        lives.gameObject.SetActive(false);
    }

    void hideButtonPrompts()
    {
        if (buttonPrompts.activeInHierarchy)
        {
            xText.text = tempXText;
            yText.text = tempYText;
        }
        iceBlockPrompts.SetActive(false);

        isDisplayingIceBlockPrompts = false;
    }

    void showButtonPrompts()
    {
        if (buttonPrompts.activeInHierarchy)
        {
            tempXText = xText.text;
            tempYText = yText.text;
            xText.text = "Throw Block";
            yText.text = "Drop Block";
        }
        else
        {
            iceBlockPrompts.SetActive(true);
        }

        isDisplayingIceBlockPrompts = true;
    }

    IEnumerator showTempElements()
    {
        showElements = true;
        yield return new WaitForSecondsRealtime(displayElementsDelay);
        showElements = false;
    }

}
