using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(CatmullRomSpline))]
public class CatmullRomSplineInspector : Editor
{
    private const int lineSteps = 100;
    private const float directionScale = 0.5f;

    private CatmullRomSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        spline = target as CatmullRomSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
        handleTransform.rotation : Quaternion.identity;

        int numPoints = spline.controlPoints.Length;

        for (int i = 0; i < numPoints; i++)
        {
            ShowPoint(i);
        }

        Handles.color = Color.red;
        for (int i = 0; i < numPoints - 1; i++)
        {
            Handles.DrawLine(spline.controlPoints[i], spline.controlPoints[i + 1]);
        }

        ShowDirections();

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
            float t = i / (float)lineSteps;
            point = spline.GetPoint(t);
            Handles.DrawLine(point, point + spline.GetDirection(t) * directionScale);
        }
    }

    private void ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.controlPoints[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.controlPoints[index] = handleTransform.InverseTransformPoint(point);
        }
    }

    private void UpdateLineRenderer()
    {
        spline.UpdateLineRenderer();
    }
}