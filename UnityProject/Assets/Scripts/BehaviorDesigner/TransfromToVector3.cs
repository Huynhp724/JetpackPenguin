using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class TransfromToVector3 : Action
{
    public SharedTransform trans;
    public SharedVector3 vector;

    public override TaskStatus OnUpdate() {
        vector.Value = trans.Value.position;
        return TaskStatus.Success;
    }
}
