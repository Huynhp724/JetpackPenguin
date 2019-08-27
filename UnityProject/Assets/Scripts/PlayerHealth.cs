using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject minorCheckPoints;

    PlayerStats stats;
    ScreenFader fader;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        fader = FindObjectOfType<ScreenFader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseALife() {
        if (stats.GetIsDead())
        {
            if (stats.lives > 1)
            {
                stats.lives -= 1;
            }
            else
            {
                stats.lives -= 1;
                stats.SetIsDead(true);

            }
        }
    }

    public void LoseEntireLives(bool checkPoint) {
        stats.lives = 0;
        stats.SetIsDead(true);
        if (checkPoint)
        {
            float minDistance = 100000f;
            GameObject point = null;

            for (int i = 0; i < minorCheckPoints.transform.childCount; i++) {
                float check = Vector3.Distance(gameObject.transform.position, minorCheckPoints.transform.GetChild(i).transform.position);
                if (check < minDistance) {
                    minDistance = check;
                    point = minorCheckPoints.transform.GetChild(i).gameObject;
                }
            }

            fader.Fade("out");
            StartCoroutine(Fade(point));

        }
    }

    IEnumerator Fade(GameObject go) {
        yield return new WaitForSeconds(1f);
        fader.Fade("in");
        gameObject.transform.position = go.transform.position;
    }
}
