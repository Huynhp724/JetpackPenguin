using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Transform spawnPosition;
    public PathCreator pathToFollow;

    Transform startPositionOfPath;

    public void Spawn() {
        GameObject go = Instantiate(enemyToSpawn, spawnPosition);
        PathFollower path = go.GetComponent<PathFollower>();
        path.followPath = false;
        path.pathCreator = pathToFollow;

        startPositionOfPath = pathToFollow.gameObject.transform.GetChild(0);

        // make Ai set to go to path

    }
}
