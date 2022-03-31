using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    private float _zoomLerp = 0;
    private float _zoomCountdown = 1;
    public float _zoomSpeed = 0.2f;
    public bool _isLerping = false;
    public bool HasZoomed = false;

    private Vector3 posA, posB;

    private Quaternion rotA, rotB;

    float doubleClickStart = 0;

    public int itemAxis = 0;

    public GameObject ZoomPos = null;

    private PlayerAxisScript AxisScript = null;

    public bool isCamera = false;
    private void Awake()
    {
        AxisScript = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
    }

    //Pour faire le double clic
    void OnMouseUp()
    {

        if ((Time.time - doubleClickStart) < 0.3f)
        {
            this.OnSimpleClick();
            doubleClickStart = -1;
        }


    }

    void OnSimpleClick()
    {
        //Rapproche le doc du joueur ou le remet à sa place en cliquant dessus
        if (AxisScript.IDCurrentAxis == itemAxis)
        {
            if (!_isLerping)
            {
                if (!HasZoomed)
                {
                    if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == true)
                    {
                        posA = _originalPosition;
                        posB =  ZoomPos.transform.position;
                        rotA = _originalRotation;
                        rotB = ZoomPos.transform.rotation;
                        _zoomCountdown = 1;
                        _zoomLerp = 0;
                        ZoomPos.GetComponent<ZoomPoint>().IsEmpty = false;
                        _isLerping = true;
                        AxisScript.HasItem = true;
                    }

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
        doubleClickStart = Time.time;
    }
    private void Update()
    {
        if (!HasZoomed && !_isLerping)
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;

        }
        else
        {
            if(GetComponent<Rigidbody>())
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
                HasZoomed = !HasZoomed;
                if(!HasZoomed)
                    AxisScript.HasItem = false;
                _isLerping = false;
            }

            if(!isCamera)
            {
                transform.position = Vector3.Lerp(posA, posB, _zoomLerp);
                transform.rotation = Quaternion.Slerp(rotA, rotB, _zoomLerp);
            }
            else
            {
                transform.position = Vector3.Lerp(posA, posB, _zoomLerp);
            }


            _zoomLerp = 1f - _zoomCountdown;
        }

        //Remet l'objet à sa place si le joueur change d'axe
        if (AxisScript.IDCurrentAxis != itemAxis)
        {
            if (HasZoomed)
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
                }
            }
        }

        if (Input.GetKeyDown("s"))
        {
            if (HasZoomed)
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
                }
            }
        }
    }
}
