using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    private WorldManager wm;
    private string id;

    void Start()
    {
        id = transform.position.ToString();
        wm = FindObjectOfType<WorldManager>();
        if (wm.checkCollected(id))
        {
            Destroy(gameObject);
        }
    }

    public void destroyDome()
    {
        wm.setCollected(id);
        Destroy(gameObject);
    }
}
