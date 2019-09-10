using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class WithinDistance : Action
{
    public SharedVector3 targetPosition;
    public float maxDistnace;


    public override TaskStatus OnUpdate()
    {
        float distance = Vector3.Distance(transform.position, targetPosition.Value);
        if (distance <= maxDistnace)
        {
            return TaskStatus.Success;
        }
        else {
            Debug.Log(distance);
            return TaskStatus.Failure;
        }
    }

}
