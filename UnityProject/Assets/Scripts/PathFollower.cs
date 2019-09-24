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

    public Vector3 lastPosition;

    float distanceTraveled;
    BehaviorTree bTree;
    EnemyHealth health;
    // Start is called before the first frame update
    void Start()
    {
        bTree = GetComponent<BehaviorTree>();
        health = GetComponent<EnemyHealth>();
        speed = health.stats.speed;
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
        else {
            lastPosition = pathCreator.path.GetClosestPointOnPath(transform.position);
            //Debug.Log(lastPosition);

        }
    }

    public void SetFollowPath(bool follow) {
        followPath = follow;
    }

    public void GetLastPathPosition() {
        bTree.SetVariableValue("VectorPosition", lastPosition);
    }
}
