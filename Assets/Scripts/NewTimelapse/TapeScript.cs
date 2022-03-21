using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeScript : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    [SerializeField] private float _zoomLerp = 0;
    [SerializeField] private float _zoomCountdown = 1;
    [SerializeField] private float _zoomSpeed = 0.2f;
    [SerializeField] private bool _isLerping = false;
    [SerializeField] private bool _hasZoomed = false;
    public AudioClip sound = null;

    private Vector3 posA, posB;

    private Quaternion rotA, rotB;

    float doubleClickStart = 0;
    //Pour faire le double clic
    void OnMouseUp() 
    {

        if ((Time.time - doubleClickStart) < 0.3f) 
        { 
            this.OnDoubleClick(); 
            doubleClickStart = -1; 

        } 
        else 
        { 
            doubleClickStart = Time.time; 
        } 
    }

    void OnDoubleClick()
    {
        //Rapproche la casette du joueur ou la remet à sa place en cliquant dessus
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5)
        {
            if (!_isLerping)
            {
                if (!_hasZoomed)
                {
                    if (GameObject.Find("TapePosition").GetComponent<ZoomPoint>().IsEmpty == true)
                    {
                        posA = _originalPosition;
                        posB = GameObject.Find("TapePosition").transform.position;
                        rotA = _originalRotation;
                        rotB = GameObject.Find("TapePosition").transform.rotation;
                        _zoomCountdown = 1;
                        _zoomLerp = 0;
                        GameObject.Find("TapePosition").GetComponent<ZoomPoint>().IsEmpty = false;
                        _isLerping = true;
                        GameObject.Find("Player").GetComponent<PlayerAxisScript>().HasTape = true;
                    }

                }
                else
                {
                    if (GameObject.Find("TapePosition").GetComponent<ZoomPoint>().IsEmpty == false)
                    {
                        posB = _originalPosition;
                        posA = GameObject.Find("TapePosition").transform.position;
                        rotB = _originalRotation;
                        rotA = GameObject.Find("TapePosition").transform.rotation;
                        _zoomCountdown = 1;
                        _zoomLerp = 0;
                        GameObject.Find("TapePosition").GetComponent<ZoomPoint>().IsEmpty = true;
                        _isLerping = true;
                        GameObject.Find("Player").GetComponent<PlayerAxisScript>().HasTape = false;
                    }
                }
            }

        }
    }

    private void OnMouseDown()
    {
        AudioSource Radio = GameObject.Find("Radio").GetComponent<AudioSource>();
        if (!_hasZoomed)
        {
            if (GetComponent<DragObjects>())
            {
                GetComponent<DragObjects>().IsDragable = true;
            }
        }
        if(Radio.clip == sound)
        {
            Radio.Stop();
            Radio.clip = null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(!_hasZoomed && !_isLerping)
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;

        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            if (GetComponent<DragObjects>())
                GetComponent<DragObjects>().IsDragable = false;
        }
        //Lerp de la casette
        if (_isLerping)
        {
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);

            if (_zoomCountdown == 0)
            {
                _hasZoomed = !_hasZoomed;
                _isLerping = false;
            }

            transform.position = Vector3.Lerp(posA, posB, _zoomLerp);
            transform.rotation = Quaternion.Slerp(rotA, rotB, _zoomLerp);

            _zoomLerp = 1f - _zoomCountdown;
        }
        //Remet la casette à sa place si le joueur change d'axe
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis != 5)
        {
            if (_hasZoomed)
            {
                if (GameObject.Find("TapePosition").GetComponent<ZoomPoint>().IsEmpty == false)
                { 
                    posB = _originalPosition;
                    posA = GameObject.Find("TapePosition").transform.position;
                    rotB = _originalRotation;
                    rotA = GameObject.Find("TapePosition").transform.rotation;
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    GameObject.Find("TapePosition").GetComponent<ZoomPoint>().IsEmpty = true;
                    _isLerping = true;
                }
            }

        }
    }
}
