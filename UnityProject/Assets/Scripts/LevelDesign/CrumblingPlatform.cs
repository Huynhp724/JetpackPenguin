﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] float timeToCrumble = 1f;
    [SerializeField] float timeToComeBack = 5f;
    [SerializeField] Animator animator;

    private BoxCollider[] colliders;
    private SkinnedMeshRenderer[] renderers;

    private AudioScript audioScript;

    private void Awake()
    {
        audioScript = GetComponent<AudioScript>();

        colliders = GetComponents<BoxCollider>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        
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
        audioScript.PlaySound(0);
        animator.Play("NewCrumblingPlatformAnimation");
        //animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSecondsRealtime(timeToCrumble);
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
        yield return new WaitForSecondsRealtime(.5f);
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
