using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CamCopyCreator : MonoBehaviour, IClickable
{
    public GameObject copyCam = null;
    private bool clicked = false;

    private bool isClickable = true;
    public Action OnClicked;
    public Action GetOnClicked { get { return OnClicked; } set { OnClicked = value; } }

    public void OnMouseDrag()
    {
        if(!GetComponent<ZoomScript>().HasZoomed && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if(!isClickable)
            {
                OnClicked?.Invoke();
                return;
            }
            if (!clicked)
            {
                clicked = true;
                GenerateCopyCam();
            }
        }

    }

    public void GenerateCopyCam()
    {
        GameObject cam = Instantiate(copyCam, transform.position + new Vector3(0.1f,0,-0.1f), Quaternion.Euler(0, 30, 0));
        cam.GetComponent<PanelImageData>().Image = GetComponent<PanelImageData>().Image;
        cam.GetComponentInChildren<Image>().sprite = GetComponent<PanelImageData>().Image;
        cam.GetComponent<PanelImageData>().ID = GetComponent<PanelImageData>().ID;
        cam.GetComponent<DragObjects>().StartDrag();

    }

    public void OnMouseUp()
    {
        clicked = false;
    }

    public void SetClickable(bool isClickable)
    {
        this.isClickable = isClickable;
    }
}
