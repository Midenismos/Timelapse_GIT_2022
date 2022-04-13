using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CamCopyCreator : MonoBehaviour
{
    public GameObject copyCam = null;
    private bool clicked = false;

    public void OnMouseDrag()
    {
        if(!GetComponent<ZoomScript>().HasZoomed)
        {
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
        cam.GetComponent<DragObjects>().StartDrag();

    }

    public void OnMouseUp()
    {
        clicked = false;
    }
}
