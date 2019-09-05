using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class GameObjectToTransform : Action
{
    public SharedGameObject obj;
    public SharedTransform transform;

    public override void OnStart()
    {
        
    }

    public override TaskStatus OnUpdate()
    {
        transform.Value = obj.Value.transform;
        return TaskStatus.Success;
    }
}
