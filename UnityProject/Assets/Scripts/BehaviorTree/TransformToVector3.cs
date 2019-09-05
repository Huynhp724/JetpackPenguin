using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class TransformToVector3 : Action
{
    public SharedTransform transform;
    public SharedVector3 vector3Pos;

    public override TaskStatus OnUpdate()
    {
        vector3Pos.Value = transform.Value.position;
        return TaskStatus.Success;
    }
}
