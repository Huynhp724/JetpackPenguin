using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject minorCheckPoints;
    public float invincibilityFrames;

    bool isInvincible = false;
    PlayerStats stats;
    ScreenFader fader;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        fader = FindObjectOfType<ScreenFader>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.GetIsDead()) {
            Debug.Log("I am dead");
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

            }
            else {
                if (stats.lives > 1)
                {
                    stats.lives -= 1;
                    stats.hitPoints = 3;
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

    IEnumerator Invincible() {
        yield return new WaitForSeconds(invincibilityFrames);
        isInvincible = false;
    }

    public void LoseEntireLives(bool checkPoint) {
        Debug.Log("Lose all your lives");
        stats.lives = 0;
        stats.SetIsDead(true);
        if (checkPoint)
        {
            FindClosestMinorCheckpoint();

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
        StartCoroutine(Fade(point));
    }

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
    }
}
