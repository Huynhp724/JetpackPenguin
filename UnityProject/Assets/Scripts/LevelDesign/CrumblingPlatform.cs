using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] float timeToCrumble = 1f;
    [SerializeField] float timeToComeBack = 5f;

    private BoxCollider[] colliders;
    private MeshRenderer[] renderers;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //PlayerController player = collision.GetComponentInParent<PlayerController>();
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(crumble());
        }
    }

    IEnumerator crumble()
    {
        yield return new WaitForSecondsRealtime(timeToCrumble);
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Renderer render in renderers)
        {
            render.enabled = false;
        }
        yield return new WaitForSecondsRealtime(timeToComeBack);
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (Renderer render in renderers)
        {
            render.enabled = true;
        }
    }
}
