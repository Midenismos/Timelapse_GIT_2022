using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public Vector3 _originalPosition;
    public Quaternion _originalRotation;

    public float _zoomLerp = 0;
    public float _zoomCountdown = 1;
    public float _zoomSpeed = 0.2f;
    public bool _isLerping = false;
    public bool HasZoomed = false;

    public Vector3 posA, posB;

    public Quaternion rotA, rotB;

    float clickStart = 0;

    public int itemAxis = 0;

    public GameObject ZoomPos = null;

    private PlayerAxisScript AxisScript = null;

    public bool isCamera = false;

    private bool clicked = false;
    public bool IsZoomable = true;
    private void Awake()
    {
        AxisScript = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
    }

    //Pour faire le double clic
    void OnMouseUp()
    {
        clicked = false;
        if ((Time.time - clickStart) < 0.3f)
        {
            this.OnSimpleClick();
            clickStart = -1;
        }


    }

    void OnSimpleClick()
    {
        //Rapproche le doc du joueur ou le remet à sa place en cliquant dessus
        if (AxisScript.IDCurrentAxis == itemAxis && !AxisScript.IsInTI)
        {
            if (!_isLerping)
            {
                if (AxisScript.HasItem && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>() != this)
                {
                    DezoomCurrentItem();

                }
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == true)
                {
                    posA = _originalPosition;
                    posB = ZoomPos.transform.position;
                    rotA = _originalRotation;
                    rotB = ZoomPos.transform.rotation;
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    ZoomPos.GetComponent<ZoomPoint>().IsEmpty = false;
                    _isLerping = true;
                    AxisScript.HasItem = true;
                    AxisScript.CurrentHoldItem = this.gameObject;

                }
                /*else
                {
                    if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                    {
                        posB = _originalPosition;
                        posA = ZoomPos.transform.position;
                        rotB = _originalRotation;
                        rotA = ZoomPos.transform.rotation;
                        _zoomCountdown = 1;
                        _zoomLerp = 0;
                        ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
                        _isLerping = true;
                        AxisScript.HasItem = false;
                    }
                }*/
            }

        }
    }

    private void OnMouseDown()
    {
        if (IsZoomable)
        {
            clicked = true;
            clickStart = Time.time;
        }

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>().clicked != true)
            {
                RaycastHit hit;
                Ray ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag != "Button")
                        DezoomCurrentItem();
                }
            }
        }
        if (!HasZoomed && !_isLerping)
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;

        }
        else
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().isKinematic = true;
            if (GetComponent<DragObjects>())
                GetComponent<DragObjects>().IsDragable = false;
        }
        //Lerp du papier
        if (_isLerping)
        {
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);

            if (_zoomCountdown == 0)
            {
                _isLerping = false;
                HasZoomed = !HasZoomed;
                if (tag == "Tape")
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<DragObjects>().IsDragable = true;
                }
            }

            if (!isCamera)
            {
                transform.position = Vector3.Lerp(posA, posB, _zoomLerp);
                transform.rotation = Quaternion.Slerp(rotA, rotB, _zoomLerp);
            }
            else
            {
                transform.parent.position = Vector3.Lerp(posA, posB, _zoomLerp);
            }


            _zoomLerp = 1f - _zoomCountdown;
        }

        //Remet l'objet à sa place si le joueur change d'axe
        if (AxisScript.CurrentHoldItem)
        { 
            if (AxisScript.IDCurrentAxis != AxisScript.CurrentHoldItem.GetComponent<ZoomScript>().itemAxis)
            {
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                {
                    DezoomCurrentItem();
                }
            }
        }

        if (Input.GetKeyDown("s"))
        {
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.gameObject.tag != "Tape")
            {
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                {
                    DezoomCurrentItem();
                }
            }
        }
    }

    public void DezoomCurrentItem()
    {
        ZoomScript currentItem = AxisScript.CurrentHoldItem.GetComponent<ZoomScript>();
        currentItem.posB = currentItem._originalPosition;
        currentItem.posA = currentItem.ZoomPos.transform.position;
        currentItem.rotB = currentItem._originalRotation;
        currentItem.rotA = currentItem.ZoomPos.transform.rotation;
        currentItem._zoomCountdown = 1;
        currentItem._zoomLerp = 0;
        currentItem.ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
        currentItem._isLerping = true;
        AxisScript.HasItem = false;
        AxisScript.CurrentHoldItem = null;

    }
}
