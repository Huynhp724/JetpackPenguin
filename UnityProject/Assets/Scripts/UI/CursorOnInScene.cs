using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOnInScene : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible == false)
            Cursor.visible = true;
    }
}
