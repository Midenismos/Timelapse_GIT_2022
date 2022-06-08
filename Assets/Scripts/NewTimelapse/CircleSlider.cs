using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSlider : MonoBehaviour
{
    private float SceneWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;
    private Quaternion Rotation;
    public float value = 1;

    private void Start()
    {
        SceneWidth = Screen.width;
    }

    private void OnMouseDown()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5 && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick)
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }

    }

    private void OnMouseDrag()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5 && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick)
        {
            float CurrentDistanceBetweenMousePositions = (Input.mousePosition - PressPoint).x;
            Rotation = StartRotation * Quaternion.Euler(Vector3.up * (CurrentDistanceBetweenMousePositions / SceneWidth) * 360);
            Rotation.x = ClampAngle(Rotation.x *100, -120, 120);
            transform.localRotation = Quaternion.Euler(Rotation.x, 90,90);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
//9