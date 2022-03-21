﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjects : MonoBehaviour
{

    private Vector3 mOffset;

    private float mZCoord;

    public bool IsDragable = true;
    private void OnMouseDown()
    {
        if(IsDragable)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            mZCoord = GameObject.Find("Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }

    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return GameObject.Find("Camera").GetComponent<Camera>().ScreenToWorldPoint(mousePoint);
    }
    private void OnMouseDrag()
    {
        if (IsDragable)
        {
            transform.position = GetMouseWorldPos() + mOffset;
        }
    }

    public void OnMouseUp()
    {
        if (IsDragable)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
