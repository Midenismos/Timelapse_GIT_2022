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

    private Vector3 _targetRotation;
    private Vector3 _targetPosition;
    private Vector3 _targetCamRotation;

    public int IDCurrentAxis = 0;
    [SerializeField] private float _moveLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _rotationSpeed = 0.2f;
    private bool _isLerping = false;
    public bool HasItem = false;
    private GameObject cam;
    public bool IsInTI = false;

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
                if (!_isLerping && !IsInTI)
                {
                    IDCurrentAxis += 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetRotation = _axisRotation[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    if (IDCurrentAxis == 5 || IDCurrentAxis == 4)
                        GameObject.Find("ButtonsPanel").GetComponent<CamManager>().StartSliderLerp();
                }
            }
            if (Input.GetKeyDown("q"))
            {
                if (!_isLerping && !IsInTI)
                {
                    IDCurrentAxis -= 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetRotation = _axisRotation[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    if (IDCurrentAxis == 5 || IDCurrentAxis == 4)
                        GameObject.Find("ButtonsPanel").GetComponent<CamManager>().StartSliderLerp();
                }
            }
            if (Input.GetKeyDown("s"))
            {
                if (!_isLerping && !IsInTI && !HasItem)
                {
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetPosition = new Vector3(cam.transform.localPosition.x, 1, 4.5f);
                    _targetCamRotation = new Vector3(90, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
                    _isLerping = true;
                    IsInTI = true;
                }
            }
            if (Input.GetKeyDown("z"))
            {
                if (!_isLerping && IsInTI)
                {
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    IsInTI = false;
                }
            }
        }


        //Gère le lerp des sons lors d'un changement temporel

        if (_isLerping)
        {
            _rotationCountdown = Mathf.Clamp(_rotationCountdown - Time.unscaledDeltaTime * _rotationSpeed, 0f, 1f);

            if (_rotationCountdown == 0)
            {
                _currentAxisRotation = transform.rotation.eulerAngles;
                _currentCamRotation = cam.transform.rotation.eulerAngles;
                _currentCamPosition = cam.transform.localPosition;
                _isLerping = false;
            }

            transform.rotation = Quaternion.Slerp( Quaternion.Euler(_currentAxisRotation), Quaternion.Euler(_targetRotation), _moveLerp);
            cam.transform.localPosition = Vector3.Lerp(_currentCamPosition, _targetPosition, _moveLerp);
            cam.transform.rotation = Quaternion.Slerp(Quaternion.Euler(_currentCamRotation), Quaternion.Euler(_targetCamRotation), _moveLerp);

            _moveLerp = 1f - _rotationCountdown;
        }
    }
}
