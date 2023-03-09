﻿using System.Collections;
using UnityEngine;

class Raycast : MonoBehaviour
{
    [SerializeField]
    public Camera playerCamera;
    public Transform raycastOrigin;
    public float range;
    public float renderDuration;
    public EyeTracker eyeTracker;
    public bool winking;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (winking ? !eyeTracker.GetWinking() : !eyeTracker.GetDoubleBlinking()) return;
        
        LogHelper.Success("Raycast tries to select. ");
        
        _lineRenderer.SetPosition(0, raycastOrigin.position);
        var rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out var hit, range))
        {
            _lineRenderer.SetPosition(1, hit.point);
            // Selecting Action Starts: 
            Destroy(hit.transform.gameObject);
            // Selecting Action Ends. 
        }
        else
        {
            _lineRenderer.SetPosition(1, rayOrigin + (playerCamera.transform.forward * range));
        }

        StartCoroutine(RenderRaycast());
    }

    private IEnumerator RenderRaycast()
    {
        _lineRenderer.enabled = true;
        yield return new WaitForSeconds(renderDuration);
        // ReSharper disable once Unity.InefficientPropertyAccess
        _lineRenderer.enabled = false;
    }
}