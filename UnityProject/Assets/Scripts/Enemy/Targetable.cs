using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    private Renderer renderer;
    private PlayerAbilities player;
    private bool isTargeted = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerAbilities>();
        renderer = GetComponent<Renderer>();
        if(renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(renderer.isVisible && Vector3.Distance(transform.position, player.transform.position) <= player.getMaxTargetRange())
        {
            if (!isTargeted)
            {
                player.addToTargets(gameObject);
                isTargeted = true;
            }
        }
        else if(isTargeted)
        {
            player.removeFromTargets(gameObject);
            isTargeted = false;
        }
    }

    private void OnDestroy()
    {
        if (isTargeted)
        {
            player.removeFromTargets(gameObject);
        }
    }
}
