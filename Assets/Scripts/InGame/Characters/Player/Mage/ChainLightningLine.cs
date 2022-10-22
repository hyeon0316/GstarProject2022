using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.enabled = false;
    }
}
