using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class GameObjectToTransform : Action
{
    public SharedGameObject obj;
    public SharedTransform tran;

    public override TaskStatus OnUpdate()
    {
        tran.Value = obj.Value.transform;
        return TaskStatus.Success;
    }
}
