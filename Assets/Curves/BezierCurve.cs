using UnityEngine;

public class BezierCurve : MonoBehaviour {

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

    public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}
	
	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}
}