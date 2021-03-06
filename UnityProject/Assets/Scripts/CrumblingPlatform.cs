﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] float timeToCrumble = 1f;
    [SerializeField] float timeToComeBack = 5f;

    private BoxCollider[] colliders;
    private MeshRenderer renderer;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        StartCoroutine(crumble(collision));
    }

    IEnumerator crumble(Collider collision)
    {
        yield return new WaitForSecondsRealtime(timeToCrumble);
        if (collision.gameObject.tag == "Player")
        {
            foreach (BoxCollider collider in colliders)
            {
                collider.enabled = false;
            }
            renderer.enabled = false;
        }
        yield return new WaitForSecondsRealtime(timeToComeBack);
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = true;
        }
        renderer.enabled = true;
    }
}
