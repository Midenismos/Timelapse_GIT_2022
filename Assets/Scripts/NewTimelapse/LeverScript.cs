using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeverScript : MonoBehaviour
{
    [SerializeField] private Slider _slider = null;
    [SerializeField] private InterfaceAnimManager _animManager = null;

    /*private float SceneHeight;
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
            Rotation.z = ClampAngle(Rotation.z * 60, -20, 20);
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
    }*/

    private void Update()
    {

        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 2)
        {
            if (_animManager.currentState == CSFHIAnimableState.disappeared)
                _animManager.startAppear();
        }
        else
        {
            if (_animManager.currentState == CSFHIAnimableState.appeared)
                _animManager.startDisappear();
        }
            //Change la vitesse du vaisseau en fonction du slider.
        if (_slider.gameObject.activeInHierarchy)
        {
            if (_slider.value >= 1.3f && _slider.value < 1.5f)
                GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.SLOW;
            else if(_slider.value >= 1f && _slider.value < 1.3f)
                GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.BACKWARDSLOW;
            else if(_slider.value >= 0.75f && _slider.value < 1f)
                GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.BACKWARDNORMAL;
            else if(_slider.value < 0.75f)
                GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.BACKWARDFAST;
            else if (_slider.value >= 1.75f )
                GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.FAST;
            else
                GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed = SpeedType.NORMAL;
        }


    }
}
