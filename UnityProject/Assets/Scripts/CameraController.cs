using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float AngleVSpd = 2f;
    public float AngleHSpd = 2f;
    public float Accel = 10f;
    public float ZoomSpd = 5f;

    [Range(0, 65)]
    public float maxViewAngle;
    [Range(-50, 0)]
    public float minViewAngle;

    public bool invertY;

    public bool useOffset;
    public Vector3 offset;

    public Transform pivot;

    private GameManager gameManager;

    void Start()
    {
        if (!useOffset)
        {
            offset = player.transform.position - transform.position;
        }

        pivot.transform.position = player.transform.position;
        pivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {


    }

    private void FixedUpdate()
    {

    }

    void LateUpdate()
    {
        pivot.transform.position = player.transform.position;

        float hori = (Input.GetAxis(gameManager.cameraX) + Input.GetAxis("Mouse X")) * AngleHSpd;// + Input.GetAxis("Mouse X")) * AngleHSpd;
        pivot.transform.Rotate(0, hori, 0);

        float vert = (Input.GetAxis(gameManager.cameraY) + Input.GetAxis("Mouse Y") * -1) * AngleVSpd;// + Input.GetAxis("Mouse Y") * -1) * AngleVSpd;
        if (invertY)
        {
            pivot.transform.Rotate(vert, 0, 0);
        }
        else
        {
            pivot.transform.Rotate(-vert, 0, 0);
        }

        //Limit up/down camera rotation
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle, pivot.eulerAngles.y, 0);
        }

        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
        {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, pivot.eulerAngles.y, 0);
        }

        //Zoom In
        if (Input.GetKey(KeyCode.O))
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, ZoomSpd * Time.deltaTime);
            offset = player.transform.position - transform.position;
        }
        //Zoom Out
        if (Input.GetKey(KeyCode.U))
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -1 * ZoomSpd * Time.deltaTime);
            offset = player.transform.position - transform.position;
        }

        float xAngle = pivot.eulerAngles.x;
        float yAngle = pivot.eulerAngles.y;


        //FIX ROTATION LATER
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = Vector3.Slerp(transform.position, player.transform.position - (rotation * offset), Accel * Time.deltaTime);
        /*Vector3 tmp = gameObject.transform.position;

        tmp.x = (Mathf.Cos(AngleV * (Mathf.PI / 180)) * Mathf.Sin(AngleH * (Mathf.PI / 180))) * (Dist * cameraOffsetScale.x) + player.transform.position.x;
        tmp.z = (Mathf.Cos(AngleH * (Mathf.PI / 180)) * Mathf.Cos(AngleV * (Mathf.PI / 180))) * (Dist * cameraOffsetScale.x) + player.transform.position.z;
        tmp.y = Mathf.Sin(AngleV * (Mathf.PI / 180)) * (Dist * cameraOffsetScale.y) + player.transform.position.y;
        transform.position = Vector3.Slerp(transform.position, tmp, Accel * Time.deltaTime);*/
        if (transform.position.y < player.transform.position.y)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z), Accel * Time.deltaTime);
        }
        transform.LookAt(player.transform);
    }
}



/*void Start()
{
    Vector3 tmp = player.transform.position;
    tmp.y = tmp.y + OffsetY;
    Dist = Vector3.Distance(gameObject.transform.position, tmp);
    Debug.Log(AngleV);
    Debug.Log(AngleH);
}

private void Update()
{
    //Dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
    if (Input.GetKey(KeyCode.I))
    {
        AngleV -= AngleVSpd * Time.deltaTime;
        //gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x + AngleVSpd * Time.deltaTime, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);

    }
    if (Input.GetKey(KeyCode.K))
    {
        AngleV += AngleVSpd * Time.deltaTime;

    }
    if (Input.GetKey(KeyCode.J))
    {
        AngleH += AngleHSpd * Time.deltaTime;

    }
    if (Input.GetKey(KeyCode.L))
    {
        AngleH -= AngleHSpd * Time.deltaTime;

    }
    //Zoom In
    if (Input.GetKey(KeyCode.O))
    {
        Dist += ZoomSpd * Time.deltaTime;
    }
    //Zoom Out
    if (Input.GetKey(KeyCode.U))
    {
        Dist -= ZoomSpd * Time.deltaTime;
    }
    if (Dist <= MinDist)
    {
        Dist = MinDist;
    }
}

*/