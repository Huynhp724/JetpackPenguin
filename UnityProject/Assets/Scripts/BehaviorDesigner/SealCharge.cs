using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class SealCharge : Action
{
    public SharedTransform playerTransform;
    public float speed;
    public float timer;

    float realTimer;

    public override void OnStart() {
        realTimer = timer;
    }

    public override TaskStatus OnUpdate()
    {
       // while (realTimer > 0f)
       // {
            

           // realTimer -= Time.deltaTime;
        //}

        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        return TaskStatus.Success;

    }
}
