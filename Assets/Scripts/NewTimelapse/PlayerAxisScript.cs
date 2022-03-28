using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxisScript : MonoBehaviour
{
    [SerializeField] private Vector3[] _axisRotation;
    [SerializeField] private Vector3[] _camRotation;
    [SerializeField] private Vector3[] _camPosition;
    private Vector3 _currentAxisRotation;
    private Vector3 _currentCamRotation;
    private Vector3 _currentCamPosition;
    public int IDCurrentAxis = 0;
    [SerializeField] private float _moveLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _rotationSpeed = 0.2f;
    private bool _isLerping = false;
    public bool HasItem = false;
    private GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera");
        Cursor.lockState = CursorLockMode.Confined;
        IDCurrentAxis = 0;
        _currentAxisRotation = _axisRotation[IDCurrentAxis];
        _currentCamRotation = _camRotation[IDCurrentAxis];
        _currentCamPosition = _camPosition[IDCurrentAxis];
    }

    // Update is called once per frame
    void Update()
    {
        if(!HasItem)
        {
            if (Input.GetKeyDown("d"))
            {
                if (!_isLerping)
                {
                    IDCurrentAxis += 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _isLerping = true;
                    if (IDCurrentAxis == 1 || IDCurrentAxis == 2)
                        GameObject.Find("ButtonsPanel").GetComponent<CamManager>().StartSliderLerp();
                }
            }
            if (Input.GetKeyDown("q"))
            {
                if (!_isLerping)
                {
                    IDCurrentAxis -= 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _isLerping = true;
                    if (IDCurrentAxis == 1 || IDCurrentAxis == 2)
                        GameObject.Find("ButtonsPanel").GetComponent<CamManager>().StartSliderLerp();
                }
            }
        }


        //Gère le lerp des sons lors d'un changement temporel

        if (_isLerping)
        {
            _rotationCountdown = Mathf.Clamp(_rotationCountdown - Time.unscaledDeltaTime * _rotationSpeed, 0f, 1f);

            if (_rotationCountdown == 0)
            {
                _currentAxisRotation = _axisRotation[IDCurrentAxis];
                _currentCamRotation = _camRotation[IDCurrentAxis];
                _currentCamPosition = _camPosition[IDCurrentAxis];
                _isLerping = false;
            }

            transform.rotation = Quaternion.Slerp( Quaternion.Euler(_currentAxisRotation), Quaternion.Euler(_axisRotation[IDCurrentAxis]), _moveLerp);
            cam.transform.localPosition = Vector3.Lerp(_currentCamPosition, _camPosition[IDCurrentAxis], _moveLerp);
            cam.transform.rotation = Quaternion.Slerp(Quaternion.Euler(_currentCamRotation), Quaternion.Euler(_camRotation[IDCurrentAxis]), _moveLerp);

            _moveLerp = 1f - _rotationCountdown;
        }
    }
}
