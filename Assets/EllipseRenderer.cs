using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipseRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer = null;
    [SerializeField] private int segments = 300;
    [SerializeField] private float minRadius = 1;
    [SerializeField] private float maxRadius = 2;

    private void OnValidate()
    {
        DrawCircle();
    }

    public void UpdateParameters(float minRadius, float maxRadius)
    {
        this.minRadius = minRadius;
        this.maxRadius = maxRadius;
        DrawCircle();
    }
    public void DrawCircle()
    {
        int numberOfPoints = segments + 1;
        lineRenderer.positionCount = numberOfPoints;
        Vector3[] points = new Vector3[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = Mathf.Deg2Rad * 360f * i / segments;
            points[i] = new Vector3(Mathf.Sin(angle) * minRadius, 0, Mathf.Cos(angle) * maxRadius);
        }

        lineRenderer.SetPositions(points);
    }
}
