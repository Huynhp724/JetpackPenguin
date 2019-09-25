using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using BehaviorDesigner.Runtime;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Transform spawnPosition;
    public PathCreator pathToFollow;

    Transform startPositionOfPath;
    bool justSpawned = false;
    BehaviorTree bTree;

    private void Start()
    {
        bTree = GetComponent<BehaviorTree>();
    }

    public void Spawn() {
        GameObject go = Instantiate(enemyToSpawn, spawnPosition);
        go.transform.parent = null;
        SetUp(go);
        
        PathFollower path = go.GetComponent<PathFollower>();
        path.followPath = false;
        path.pathCreator = pathToFollow;

       

    }

    private void Update()
    {
        if (justSpawned) {
            StartCoroutine(StartMoving());
            justSpawned = false;
        }
    }

    void SetUp(GameObject newObj) {
        EnemySpawner newSpawner = newObj.GetComponent<EnemySpawner>();
        newSpawner.enemyToSpawn = enemyToSpawn;
        newSpawner.spawnPosition = spawnPosition;
        newSpawner.pathToFollow = pathToFollow;
        newSpawner.justSpawned = true;
    }

    IEnumerator StartMoving() {
        yield return new WaitForSeconds(2f);
        bTree.SendEvent("Find Path");
    }
}
