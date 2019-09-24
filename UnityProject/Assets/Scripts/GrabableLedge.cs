using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableLedge : MonoBehaviour
{
    public Transform ledgeStartT;
    public Transform ledgeEndT;

    public Vector3 GetGrabPosition(Vector3 hitPoint)
    {
        Vector3 temp = ledgeEndT.position - ledgeStartT.position;
        Vector3 temp2 = hitPoint - ledgeStartT.position;
        float maxDistance = temp.magnitude;
        float penguinDistance = temp2.magnitude;

        Vector3 grabPosition = Vector3.Lerp(ledgeStartT.position, ledgeEndT.position, penguinDistance / maxDistance);
        return grabPosition;
    }
    public Quaternion GetGrabRotation(Vector3 hitPoint)
    {
        Vector3 temp = ledgeEndT.position - ledgeStartT.position;
        Vector3 temp2 = hitPoint - ledgeStartT.position;
        float maxDistance = temp.magnitude;
        float penguinDistance = temp2.magnitude;

        Quaternion grabRot = Quaternion.Lerp(ledgeStartT.rotation, ledgeEndT.rotation, penguinDistance / maxDistance);
        return grabRot;
    }

    public Vector3 GetGrabPosition(float percent)
    {
        Vector3 grabPosition = Vector3.Lerp(ledgeStartT.position, ledgeEndT.position, percent);
        return grabPosition;
    }

    public Quaternion GetGrabRotation(float percent)
    {
        Quaternion grabRot = Quaternion.Lerp(ledgeStartT.rotation, ledgeEndT.rotation, percent);
        return grabRot;
    }
}
