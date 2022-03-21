using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSlider : MonoBehaviour
{
    private float SceneWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;
    public float value = 1;

    private void Start()
    {
        SceneWidth = Screen.width;
    }

    private void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }
        else if(Input.GetMouseButton(0))
        {
            float CurrentDistanceBetweenMousePositions = (Input.mousePosition - PressPoint).x;
            transform.rotation = StartRotation * Quaternion.Euler(Vector3.up * (CurrentDistanceBetweenMousePositions / SceneWidth) * 360);
            value = transform.rotation.eulerAngles.x;
        }*/
        value = transform.localRotation.x;

    }

    private void OnMouseDrag()
    {
        /*float CurrentDistanceBetweenMousePositions = (Input.mousePosition - PressPoint).x;
        transform.rotation = 
            
            StartRotation * Quaternion.Euler(Vector3.up * Mathf.Clamp(((CurrentDistanceBetweenMousePositions / SceneWidth) * 360), 60, 180));*/
        Vector3 mouseScreenPosition = GameObject.Find("Camera").GetComponent<Camera>().WorldToScreenPoint(Input.mousePosition);

        Vector3 lookAt = mouseScreenPosition;
            print(mouseScreenPosition);

        float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        this.transform.rotation = Quaternion.Euler(AngleDeg, 180, 90);

    }

    private void OnMouseDown()
    {
        PressPoint = Input.mousePosition;
        StartRotation = transform.rotation;


    }
}
