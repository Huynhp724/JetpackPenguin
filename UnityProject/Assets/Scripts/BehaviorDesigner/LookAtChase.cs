using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class LookAtChase : Action
{
    public SharedTransform target;
    public float rotationSpeed;
    public float speed;


    public override TaskStatus OnUpdate() {
        var lookPos = target.Value.position - transform.position;
        lookPos.y = 0f;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        return TaskStatus.Success;
    }
    
}
