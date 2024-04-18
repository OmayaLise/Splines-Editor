using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HermiteSpline))]
public class HermiteSplineInspector : Editor
{
    private const int lineSteps = 10;
    private const float directionScale = 0.5f;

    private HermiteSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        spline = target as HermiteSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.red;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        ShowDirections();

        // Use DrawPolyLine instead of DrawBezier
        Handles.color = Color.green;
        Vector3[] linePoints = new Vector3[lineSteps + 1];
        for (int i = 0; i <= lineSteps; i++)
        {
            float t = i / (float)lineSteps;
            linePoints[i] = spline.GetPoint(t);
        }
        Handles.DrawPolyLine(linePoints);
        UpdateLineRenderer();
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
        {
            point = spline.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }

    private void UpdateLineRenderer()
    {
        spline.UpdateLineRenderer();
    }
}
