using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float speed = 5.0f;
    public float xAngle, yAngle, zAngle;

    //public Rigidbody rb;
    //public Vector3 rotationVelocity;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();

        //rotationVelocity = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(xAngle, yAngle, zAngle, Space.World);

        //Quaternion deltaRotation = Quaternion.Euler(rotationVelocity * Time.deltaTime);
        //rb.MoveRotation(rb.rotation * deltaRotation);

    }
}
