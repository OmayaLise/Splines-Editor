using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteSpline : MonoBehaviour
{
    public Vector3[] points;
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
        return transform.TransformPoint(Hermite.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Hermite.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }
}

public static class Hermite
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1, float t)
    {
        float t2 = t * t;
        float t3 = t * t * t;
        float blend1 = 2 * t3 - 3 * t2 + 1;
        float blend2 = -2 * t3 + 3 * t2;
        float blend3 = t3 - 2 * t2 + t;
        float blend4 = t3 - t2;

        return blend1 * p0 + blend2 * p1 + blend3 * m0 + blend4 * m1;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1, float t)
    {
        float t2 = t * t;
        float blend1 = 6 * t2 - 6 * t;
        float blend2 = -6 * t2 + 6 * t;
        float blend3 = 3 * t2 - 4 * t + 1;
        float blend4 = 3 * t2 - 2 * t;

        return blend1 * (p1 - p0) + blend3 * m0 + blend4 * m1;
    }
}
