using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{

    private float SceneHeight;
    private Vector3 PressPoint;
    private Quaternion StartRotation;
    private Quaternion Rotation;

    private void Start()
    {
        SceneHeight = Screen.height;
    }
    //Permet d'interagit avec le levier

    private void OnMouseDown()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 2)
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }

    }

    private void OnMouseDrag()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 2)
        {
            float CurrentDistanceBetweenMousePositions = -(Input.mousePosition - PressPoint).y;
            Rotation = StartRotation * Quaternion.Euler(Vector3.forward * (CurrentDistanceBetweenMousePositions / SceneHeight) * 360);
            Rotation.z = ClampAngle(Rotation.z * 60, -30, 30);
            transform.parent.transform.localRotation = Quaternion.Euler(0f, 0f, Rotation.z);
        }
    }

    //Permet de faire un clamp avec des valeurs d'angles Euler négatives
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


    private void Update()
    {
        //Change la vitesse du vaisseau en fonction de l'angle du levier.
        if (transform.parent.transform.localRotation.eulerAngles.z > 20 && transform.parent.transform.localRotation.eulerAngles.z < 30.1)
            GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.SLOW;
        else if (transform.parent.transform.localRotation.eulerAngles.z > 329 && transform.parent.transform.localRotation.eulerAngles.z < 340)
            GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.FAST;
        else
            GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.NORMAL;

    }
}
