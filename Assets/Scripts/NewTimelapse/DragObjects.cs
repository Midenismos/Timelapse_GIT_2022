using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjects : MonoBehaviour
{

    private Vector3 mOffset;

    private float mZCoord;

    public bool IsDragable = true;
    public bool IsDragged = false;
    public bool Is3D = true;
    public bool IsEntry = false;
    [SerializeField] private MeshRenderer _interactFeedBack;
    [SerializeField] private int _axisID = 0;

    private void Awake()
    {
        try
        {
            _interactFeedBack = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        catch
        {
            _interactFeedBack = null;
        }
    }
    public void OnMouseDown()
    {
        if (tag == "PanelImage" ||GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
        {
            if (GetComponent<ZoomScript>())
            {
                if (!GetComponent<ZoomScript>().HasZoomed)
                {
                    GetComponent<DragObjects>().IsDragable = true;
                }
            }
            else
            {
                GetComponent<DragObjects>().IsDragable = true;
                if (tag == "PanelImage")
                    transform.SetParent(GameObject.Find("TI").transform, true);

            }

            if (IsDragable)
            {
                if (GetComponent<Rigidbody>())
                    GetComponent<Rigidbody>().isKinematic = true;
                mZCoord = GameObject.Find("Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position).z;
                mOffset = gameObject.transform.position - GetMouseWorldPos();
            }
        }


    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;
        if (Is3D)
            mousePoint.y = Mathf.Clamp(mousePoint.y, 150, 5000);
        else if (IsEntry)
            mousePoint.y = transform.position.y;


        return GameObject.Find("Camera").GetComponent<Camera>().ScreenToWorldPoint(mousePoint);
    }
    public void OnMouseDrag()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
        {
            if (IsDragable)
            {
                if (tag == "Minimap" || tag == "Cam")
                    transform.parent.transform.position = GetMouseWorldPos() + mOffset;
                else
                    transform.position = GetMouseWorldPos() + mOffset;

                IsDragged = true;
                GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = true;
                if (GetComponent<ZoomScript>())
                    GetComponent<ZoomScript>().enabled = true;
            }
        }
    }

    public void OnMouseUp()
    {
        if (IsDragable)
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().isKinematic = false;
        }
        IsDragged = false;
        GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = false;

    }

    private void OnMouseExit()
    {
        if (_interactFeedBack)
            _interactFeedBack.enabled = false;
    }
    private void OnMouseEnter()
    {
        if(_interactFeedBack)
            _interactFeedBack.enabled = true;
    }
}
