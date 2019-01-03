using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCube;

public class HyperCubeLineRenderer : HyperCubeBase
{
    [Header("Optional")]
    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer[] lineRenderers;

    private void Start()
    {
        base.Start();
        GenerateLineRenderers();
    }

    void Update()
    {
        base.Update();
        UpdateLineRenderers();
    }

    void GenerateLineRenderers()
    {
        int count = root.indicesGroups.Count;
        lineRenderers = new LineRenderer[count];

        for (int i = 0; i < count; i++)
        {
            LineRenderer lineRenderer;

            if (lineRendererPrefab != null)
                lineRenderer = InstantiateLineRendererFromPrefab();
            else
                lineRenderer = InstantiateGenericLineRenderer();

            lineRenderer.positionCount = root.indicesGroups[i].indices.Length;
            lineRenderers[i] = lineRenderer;
        }
    }

    LineRenderer InstantiateGenericLineRenderer()
    {
        GameObject lineRendererGO = new GameObject("LineRenderer");
        lineRendererGO.transform.SetParent(transform);
        LineRenderer lineRenderer = lineRendererGO.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.material = data.material;

        return lineRenderer;
    }

    LineRenderer InstantiateLineRendererFromPrefab()
    {
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform);
        return lineRenderer;
    }

    void UpdateLineRenderers()
    {
        int count = root.indicesGroups.Count;
        for (int i = 0; i < count; i++)
        {
            lineRenderers[i].startWidth = data.lineSize;
            lineRenderers[i].endWidth = data.lineSize;
            lineRenderers[i].colorGradient = data.gradient;

            int[] indices = root.indicesGroups[i].indices;
            Vector3[] linePositions = new Vector3[indices.Length];
            for (int j = 0; j < indices.Length; j++)
            {
                int index = indices[j];
                linePositions[j] = vertices[index] + transform.position;
            }

            lineRenderers[i].SetPositions(linePositions);
        }
    }
}
