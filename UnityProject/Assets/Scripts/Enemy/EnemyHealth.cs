using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyStats stats;
    public Transform squishPosition;
    public GameObject freezedStatePrefab;
    public GameObject deathParticles;

    [HideInInspector] public bool isFreezing = false;

    int hp;
    bool isDead, freezing;
    float maxTimer, timer;

    
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        hp = stats.hp;
        maxTimer = stats.freezeTimerPerHP;
        timer = stats.freezeTimerPerHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreezing) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                IsFrozen();
                timer = maxTimer;
                
            }
        }

    }

    public void IsFrozen() {
        CreateIceBlock();
        Destroy(gameObject);
        
    }

    public void LoseHp() {
        if (!isDead) {
            if (hp > 1)
            {
                hp -= 1;
            }
            else {
                hp = 0;
                isDead = true;

                if (isFreezing)
                {
                    //CreateIceBlock();
                    LoseLife();
                }
                else
                {
                    LoseLife();
                }
            }
        }
    }

    public void LoseLife() {
        if(GetComponent<EnemySpawner>() != null)
        {
            GetComponent<EnemySpawner>().Spawn();
        }
        Destroy(gameObject);
        
    }

    public void CreateIceBlock() {
        print("Creating an Iceblock");
        GameObject newObj = Instantiate(freezedStatePrefab, transform.position, transform.rotation);
        newObj.transform.parent = null;
        //newObj.transform.localPosition = transform.localPosition;
    }

    public void SetFreeze(bool freeze) {
        isFreezing = freeze;

    }

    public void IsSquished() {
        
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        //-----------Death Audio------------------------------------
        GetComponent<AudioScript>().PlaySound(0);
        //-----------Plays Map Wide on Scene Exit -----------------
    }
}
