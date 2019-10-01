using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public GameObject rootModelObj;
    public float invincibilityFrames;
    public float respawnTime;

    bool isInvincible = false;
    GameObject minorCheckPoints;

    PlayerStats stats;
    ScreenFader fader;
    FlickerRenderer flick;
    PlayerInteraction playerInteraction;
    Animator anim;
    PlayerController playerController;

    

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        fader = FindObjectOfType<ScreenFader>();
        flick = GetComponentInChildren<FlickerRenderer>();
        playerInteraction = GetComponent<PlayerInteraction>();
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (fader == null) {
            fader = FindObjectOfType<ScreenFader>();
        }
        if (stats.GetIsDead()) {
            Debug.Log("I am dead");
            flick.SwitchAllRenderers(false);
            stats.ResetStats();
            FindMajorCheckpoint();
        }

        if (minorCheckPoints == null) {
            try
            {
                minorCheckPoints = GameObject.FindGameObjectWithTag("MinorCheckpoint");
            }
            catch { }
        }
    }

    public void GainLife() {
        if (!stats.GetIsDead()) {
            stats.lives++;
            if (stats.lives > 2) {
                stats.lives = 2;
                // change UI
            }
        }
    }

    public void GainHitpoint() {
        if (!stats.GetIsDead()) {
            stats.hitPoints++;
            if (stats.hitPoints > 3) {
                stats.hitPoints = 3;
                // chnage UI
            }
        }
    }


    public void LoseALife(bool checkPoint) {
        Debug.Log("Lose a life");
        if (!stats.GetIsDead())
        {
            if (stats.lives > 1)
            {
                stats.lives -= 1;

                if (checkPoint)
                {
                    FindClosestMinorCheckpoint();
                    return;

                }
            }
            else
            {
                stats.lives -= 1;
                stats.hitPoints = 0;
                stats.SetIsDead(true);
                return;

            }
            isInvincible = true;
            //rb.AddForce(Vector3.forward * -50f, ForceMode.Impulse);
            StartCoroutine(Invincible());
        }
    }

    public void LoseHitpoint() {
        Debug.Log("Lose a health point");
        if (!stats.GetIsDead() && !isInvincible) {
            if (stats.hitPoints > 1)
            {
                stats.hitPoints -= 1;
                flick.SetFlickering();

            }
            else {
                if (stats.lives > 1)
                {
                    stats.lives -= 1;
                    stats.hitPoints = 3;
                    flick.SetFlickering();
                    FindClosestMinorCheckpoint();
                }
                else {
                    stats.lives -= 1;
                    stats.hitPoints -= 1;
                    stats.SetIsDead(true);
                }

            }
            isInvincible = true;
            //rb.AddForce(Vector3.forward * -50f, ForceMode.Impulse);
            StartCoroutine(Invincible());

        }
    }

    public void FellToDeath()
    {
        if (!stats.GetIsDead()) {
            if (stats.hitPoints > 1)
            {
                stats.hitPoints -= 1;
                flick.SetFlickering();
                FindClosestMinorCheckpoint();

            }
            else
            {
                if (stats.lives > 1)
                {
                    stats.lives -= 1;
                    stats.hitPoints = 3;
                    flick.SetFlickering();
                    FindClosestMinorCheckpoint();
                }
                else
                {
                    stats.lives -= 1;
                    stats.hitPoints -= 1;
                    stats.SetIsDead(true);
                }

            }
        }

    }

    

    IEnumerator Invincible() {
        yield return new WaitForSeconds(invincibilityFrames);
        isInvincible = false;

    }

    public void LoseEntireLives(bool checkPoint) {
        Debug.Log("Lose all your lives");
        stats.lives = 0;
        stats.hitPoints = 0;
        stats.SetIsDead(true);
        if (checkPoint)
        {
            flick.SwitchAllRenderers(false);
            stats.ResetStats();
            FindMajorCheckpoint();

        }
    }

    IEnumerator Fade(GameObject go) {
        yield return new WaitForSeconds(1f);
        fader.Fade("in");
        gameObject.transform.position = go.transform.position;
    }

    void FindClosestMinorCheckpoint() {
        float minDistance = 90000000f;
        GameObject point = null;

        for (int i = 0; i < minorCheckPoints.transform.childCount; i++)
        {
            float check = Vector3.Distance(gameObject.transform.position, minorCheckPoints.transform.GetChild(i).transform.position);
            if (check < minDistance)
            {
                minDistance = check;
                point = minorCheckPoints.transform.GetChild(i).gameObject;
            }
        }

        fader.Fade("out");
        playerController.PluckDied();
        StartCoroutine(Fade(point));
    }

    // sent here only when dead
    void FindMajorCheckpoint() {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("MajorCheckpoint");

        float minDistance = 9000000f;
        GameObject go = null;
        for (int i = 0; i < checkpoints.Length; i++) {
            float check = Vector3.Distance(gameObject.transform.position, checkpoints[i].transform.position);
            if (check < minDistance) {
                minDistance = check;
                go = checkpoints[i];
            }
        }

        gameObject.transform.position = go.transform.position;
        fader.Fade("out");
        StartCoroutine(RespawnWait());
    }

    IEnumerator RespawnWait() {
        playerInteraction.EnablePlayerController(false);
        yield return new WaitForSeconds(respawnTime);
        fader.Fade("in");
        playerInteraction.EnablePlayerController(true);
        flick.SwitchAllRenderers(true);

    }

    public void SetInvincibilty(bool invincible) {
        isInvincible = invincible;
        if (isInvincible) {
            StartCoroutine(Invincible());
        }
    }
}
