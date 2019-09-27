using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using BehaviorDesigner.Runtime;

public class PathFollower : MonoBehaviour
{
    float speed;
    public PathCreator pathCreator;
    public bool followPath = true;

    Vector3 lastPosition;

    float distanceTraveled;
    BehaviorTree bTree;
    EnemyHealth health;
    // Start is called before the first frame update
    void Start()
    {
        bTree = GetComponent<BehaviorTree>();
        health = GetComponent<EnemyHealth>();
        speed = health.stats.speed;
        lastPosition = transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        if (followPath)
        {
            distanceTraveled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTraveled);
        }

    }

    public void SetFollowPath(bool follow) {
        followPath = follow;
    }

    public void GetLastPathPosition() {
        bTree.SetVariableValue("VectorPosition", pathCreator.path.GetPoint(0));
    }

    public void StartFollowPath() {
        distanceTraveled = 0f;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTraveled);
        followPath = true;
    }
}
