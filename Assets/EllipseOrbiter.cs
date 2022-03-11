using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipseOrbiter : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] public float progress = 0;
    [SerializeField] private Ellipse ellipse = null;

    private void OnValidate()
    {
        if(ellipse)
        {
            transform.localPosition = ellipse.GetLocalPositionOnEllipse(progress);

        }
    }

    public void PlaceOnEllipse()
    {
        if (ellipse)
        {
            transform.localPosition = ellipse.GetLocalPositionOnEllipse(progress);
        }
    }
}
