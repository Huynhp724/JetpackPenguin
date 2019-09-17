using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class GameObjectToTransform : Action
{
    public SharedGameObject obj;
    public SharedTransform tran;

    public override void OnStart()
    {
        //obj.Value = GameObject.FindGameObjectWithTag("Player");
    }

    public override TaskStatus OnUpdate()

    {

        Debug.Log("Obj value is: "+obj.Value.transform);
        tran.Value = obj.Value.transform;
        Debug.Log("Transform value is: " + tran.Value);
        return TaskStatus.Success;
    }
}
