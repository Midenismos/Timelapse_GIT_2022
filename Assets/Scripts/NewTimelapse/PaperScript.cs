using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperScript : MonoBehaviour
{

    private float SceneWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;

    private void Start()
    {
        SceneWidth = Screen.width;
    }

    private void OnMouseDown()
    {
        if(GetComponent<ZoomScript>().HasZoomed)
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }

    }

    private void OnMouseDrag()
    {
        if(GetComponent<ZoomScript>().HasZoomed)
        {
            float CurrentDistanceBetweenMousePositions = (Input.mousePosition - PressPoint).x;
            transform.rotation = StartRotation * Quaternion.Euler(-Vector3.forward * (CurrentDistanceBetweenMousePositions / SceneWidth) * 360);
        }
    }
}
