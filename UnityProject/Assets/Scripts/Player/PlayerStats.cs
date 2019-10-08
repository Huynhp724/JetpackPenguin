using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerAttackStats playerAttackStats;
    public int lives;
    public int hitPoints;
    public GameObject maincam;

    int setLives, setHitPoints;

    //private static bool playerExists;

    bool isDead =false;

    public GameManager gm;

    // Likely temp for Alpha
    public float pitchShift = 1.0f;
    public bool shiftStart = false;
    public float shiftInterval = 10.0f;
    private float shiftIncrement = 0.0f;
    private float shiftVal = 0.0f;
    public int currentCrystals = 0;

    private void Update()
    {
        shiftVal += shiftIncrement;
        if (shiftStart && shiftIncrement != 0.0f)
        {
            pitchShift += 0.1f;
            shiftVal = 0.0f;
            shiftStart = false;
        }
        else if (shiftStart && shiftIncrement == 0.0f)
        {
            shiftIncrement = 0.1f;
        }
        else if (shiftVal > shiftInterval)
        {
            shiftVal = 0.0f;
            pitchShift = 1.0f;
            shiftStart = false;
            shiftIncrement = 0.0f;
        }

        if (Vector3.Distance(transform.position, maincam.transform.position) > 4f) {
            maincam.transform.localPosition = new Vector3(3f, 11f, -46f);
        }
    }

    public void AddCrystal(int x)
    {
        Debug.Log("Adding " + x + " to crystals.");
        currentCrystals += x;
        //crystalText.text = "Crystals: " + currentCrystals;
    }

    public int getCrystals()
    {
        return currentCrystals;
    }

    public bool GetIsDead() {
        return isDead;
    }

    public void SetIsDead(bool dead) {
        isDead = dead;
    }

    private void Start()
    {
        setLives = lives;
        setHitPoints = hitPoints;
        gm = FindObjectOfType<GameManager>();
    }

    public void ResetStats() {
        SetIsDead(false);
        lives = setLives;
        hitPoints = setHitPoints;
        
    }

    public float getHealthPercent()
    {
        float hpPercent = hitPoints;
        return hpPercent / setHitPoints;
    }

    public void TurnPlayerController(PlayerController controller) {
        StartCoroutine(TogglePlayerController(controller));

    }

    IEnumerator TogglePlayerController(PlayerController pc) {
        yield return new WaitForSeconds(1f);
        pc.enabled = true;
    }

    public GameManager GetGameManager()
    {
        if(gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }
        return gm;
    }

}
