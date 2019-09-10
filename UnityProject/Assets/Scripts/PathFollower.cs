using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour
{
    public float speed;
    public PathCreator pathCreator;
    public bool followPath = true;

    public Vector3 lastPosition;

    float distanceTraveled;
    // Start is called before the first frame update
    void Start()
    {
        
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
            //pathCreator.path.GetClosestPointOnPath()
        }
    }

    public void SetFollowPath(float newSpeed, bool follow) {
        speed = newSpeed;
        followPath = follow;
    }
}
