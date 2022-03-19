using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScreenScript : MonoBehaviour
{
    [SerializeField] private MeshRenderer _interactFeedBack;

    private Vector3 _originalPosition;

    [SerializeField] private float _zoomLerp = 0;
    [SerializeField] private float _zoomCountdown = 1;
    [SerializeField] private float _zoomSpeed = 0.2f;
    private bool _isLerping = false;
    private bool _hasZoomed = false;

    private Vector3 a, b;


    private void Awake()
    {
        _originalPosition = transform.position;
    }
    private void OnMouseOver()
    {
        _interactFeedBack.enabled = true;

        //Rapproche la cam du joueur ou le remet à sa place en cliquant dessus
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_isLerping)
                {
                    if (!_hasZoomed)
                    {
                        if (GameObject.Find("ZoomPosition").GetComponent<ZoomPoint>().IsEmpty == true)
                        {
                            a = _originalPosition;
                            b = GameObject.Find("ZoomPosition").transform.position;
                            _zoomCountdown = 1;
                            _zoomLerp = 0;
                            GameObject.Find("ZoomPosition").GetComponent<ZoomPoint>().IsEmpty = false;
                            _isLerping = true;
                        }

                    }
                    else
                    {
                        if (GameObject.Find("ZoomPosition").GetComponent<ZoomPoint>().IsEmpty == false)
                        {
                            b = _originalPosition;
                            a = GameObject.Find("ZoomPosition").transform.position;
                            _zoomCountdown = 1;
                            _zoomLerp = 0;
                            GameObject.Find("ZoomPosition").GetComponent<ZoomPoint>().IsEmpty = true;
                            _isLerping = true;
                        }
                    }
                }
            }
        }

    }

    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }

    private void Update()
    {
        //Lerp de la cam
        if (_isLerping)
        {
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);

            if (_zoomCountdown == 0)
            {
                _hasZoomed = !_hasZoomed;
                _isLerping = false;
            }

            transform.parent.transform.position = Vector3.Lerp(a, b, _zoomLerp);

            _zoomLerp = 1f - _zoomCountdown;
        }


        //Remet la cam à sa place si le joueur change d'axe
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis != 1)
        {
            if(GameObject.Find("ZoomPosition").GetComponent<ZoomPoint>().IsEmpty == false)
            {
                if(_hasZoomed)
                {
                    b = _originalPosition;
                    a = GameObject.Find("ZoomPosition").transform.position;
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    GameObject.Find("ZoomPosition").GetComponent<ZoomPoint>().IsEmpty = true;
                    _isLerping = true;
                }

            }
        }
    }


    /*private void Update()
    {
        if(Movie.clip != null)
        {
            //Change la vitesse du film en fonction du TimeManager
            if (!_timeManager.rewindManager.isRewinding)
                Movie.playbackSpeed = _timeManager.multiplier;
            else//Rewind le film
            {
                Movie.playbackSpeed = 0;
                Movie.time = Movie.time - 0.09f;
            }

            //Fait apparaître ou disparaître le boutton lorsque le film est en marche.
            if(!Movie.isPlaying)
                _button.SetActive(true);
            else if(Movie.time <=0.08)
            {
                _button.SetActive(true);
                if(_timeManager.rewindManager.isRewinding)
                    Movie.Stop();
            }
            if(Movie.isPlaying && Movie.time != 0.08)
            {
                _button.SetActive(false);

            }
        }
        else
            _button.SetActive(false);


    }*/

}
