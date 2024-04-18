using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline : MonoBehaviour
{
    public Vector3[] controlPoints;
    public LineRenderer lineRenderer;
    public int lineRendererResolution = 100; // Number of points to sample for the LineRenderer

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;

        // Set the number of positions in the LineRenderer based on the desired resolution
        lineRenderer.positionCount = lineRendererResolution;

        // Update the LineRenderer positions whenever control points are modified
        UpdateLineRenderer();
    }

    public void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            for (int i = 0; i < lineRendererResolution; i++)
            {
                float t = i / (float)(lineRendererResolution - 1);
                lineRenderer.SetPosition(i, GetPoint(t));
            }
        }
    }
    public Vector3 GetPoint(float t)
    {
        int numPoints = controlPoints.Length;
        int n = numPoints - 1;

        int p0Index = Mathf.Clamp(Mathf.FloorToInt(t * n), 0, n - 1);
        int p1Index = Mathf.Clamp(p0Index + 1, 0, n);
        int p2Index = Mathf.Clamp(p0Index + 2, 0, n);
        int p3Index = Mathf.Clamp(p0Index + 3, 0, n);

        float u = t * n - p0Index;

        Vector3 p0 = controlPoints[p0Index];
        Vector3 p1 = controlPoints[p1Index];
        Vector3 p2 = controlPoints[p2Index];
        Vector3 p3 = controlPoints[p3Index];

        return 0.5f * (
            (-p0 + 3f * p1 - 3f * p2 + p3) * (u * u * u) +
            (3f * p0 - 6f * p1 + 3f * p2) * (u * u) +
            (-3f * p0 + 3f * p2) * u +
            p0 + 4f * p1 + p2
        );
    }

    public Vector3 GetVelocity(float t)
    {
        int numPoints = controlPoints.Length;
        int n = numPoints - 1;

        int p0Index = Mathf.Clamp(Mathf.FloorToInt(t * n), 0, n - 1);
        int p1Index = Mathf.Clamp(p0Index + 1, 0, n);
        int p2Index = Mathf.Clamp(p0Index + 2, 0, n);
        int p3Index = Mathf.Clamp(p0Index + 3, 0, n);

        float u = t * n - p0Index;

        Vector3 p0 = controlPoints[p0Index];
        Vector3 p1 = controlPoints[p1Index];
        Vector3 p2 = controlPoints[p2Index];
        Vector3 p3 = controlPoints[p3Index];

        return 0.5f * (
            (3f * (-p0 + 3f * p1 - 3f * p2 + p3) * (u * u)) +
            (2f * (3f * p0 - 6f * p1 + 3f * p2) * u) +
            (-3f * p0 + 3f * p2)
        );
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void Reset()
    {
        controlPoints = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f),
            new Vector3(5f, 0f, 0f),
            new Vector3(6f, 0f, 0f),
            new Vector3(7f, 0f, 0f),
            new Vector3(8f, 0f, 0f),
            new Vector3(9f, 0f, 0f),
            new Vector3(10f, 0f, 0f)
        };
    }
}
